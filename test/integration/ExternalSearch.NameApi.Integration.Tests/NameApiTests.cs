using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Messages.Processing;
using CluedIn.Core.Processing;
using CluedIn.Core.Serialization;
using CluedIn.Core.Workflows;
using CluedIn.ExternalSearch;
using CluedIn.ExternalSearch.Providers.NameApi;
using CluedIn.Testing.Base.Context;
using CluedIn.Testing.Base.Processing.Actors;
using Moq;
using Xunit;

namespace ExternalSearch.NameApi.Integration.Tests
{
    public class NameApiTests : IDisposable
    {
        private TestContext testContext;

        [Fact(Skip = "GitHub Issue 829 - ref https://github.com/CluedIn-io/CluedIn/issues/829")]
        public void TestValidUserWithName()
        {
            // Arrange
            this.testContext = new TestContext();
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.Email, "jonas.k.thiesen@gmail.com");
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.PhoneNumber, "+4531671408");
            IEntityMetadata entityMetadata = new EntityMetadataPart()
            {
                Name = "Jonas Thiesen",
                EntityType = EntityType.Infrastructure.User,
                Properties = properties.Properties
            };

            var externalSearchProvider = new Mock<NameApiExternalSearchProvider>(MockBehavior.Loose);
            var clues = new List<CompressedClue>();

            externalSearchProvider.CallBase = true;

            this.testContext.ProcessingHub.Setup(h => h.SendCommand(It.IsAny<ProcessClueCommand>())).Callback<IProcessingCommand>(c => clues.Add(((ProcessClueCommand)c).Clue));

            this.testContext.Container.Register(Component.For<IExternalSearchProvider>().UsingFactoryMethod(() => externalSearchProvider.Object));
            var context = this.testContext.Context.ToProcessingContext();
            var command = new ExternalSearchCommand();
            var actor = new ExternalSearchProcessingAccessor(context.ApplicationContext);
            var workflow = new Mock<Workflow>(MockBehavior.Loose, context, new EmptyWorkflowTemplate<ExternalSearchCommand>());

            workflow.CallBase = true;

            command.With(context);
            command.OrganizationId = context.Organization.Id;
            command.EntityMetaData = entityMetadata;
            command.Workflow = workflow.Object;
            context.Workflow = command.Workflow;

            // Act
            var result = actor.ProcessWorkflowStep(context, command);
            Assert.Equal(WorkflowStepResult.Repeat.SaveResult, result.SaveResult);

            result = actor.ProcessWorkflowStep(context, command);
            Assert.Equal(WorkflowStepResult.Success.SaveResult, result.SaveResult);
            context.Workflow.AddStepResult(result);

            context.Workflow.ProcessStepResult(context, command);

            // Assert
            this.testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.True(clues.Count > 0);
        }

        // TODO Issue 170 - Unit Test Failures
        //[Fact]
        //public void TestValidUserWithoutName()
        //{
        //    // Arrange
        //    this.testContext = new TestContext();
        //    var properties = new EntityMetadataPart();
        //    properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.Email, "jonas.k.thiesen@gmail.com");
        //    properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.PhoneNumber, "+4531671408");
        //    IEntityMetadata entityMetadata = new EntityMetadataPart()
        //    {
        //        EntityType = EntityType.Infrastructure.User,
        //        Properties = properties.Properties
        //    };

        //    var externalSearchProvider = new Mock<NameApiExternalSearchProvider>(MockBehavior.Loose);
        //    var clues = new List<CompressedClue>();

        //    externalSearchProvider.CallBase = true;

        //    this.testContext.ProcessingHub.Setup(h => h.SendCommand(It.IsAny<ProcessClueCommand>())).Callback<IProcessingCommand>(c => clues.Add(((ProcessClueCommand)c).Clue));

        //    this.testContext.Container.Register(Component.For<IExternalSearchProvider>().UsingFactoryMethod(() => externalSearchProvider.Object));
        //    var context = this.testContext.Context.ToProcessingContext();
        //    var command = new ExternalSearchCommand();
        //    var actor = new ExternalSearchProcessing(context.ApplicationContext);
        //    var workflow = new Mock<Workflow>(MockBehavior.Loose, context, new EmptyWorkflowTemplate<ExternalSearchCommand>());

        //    workflow.CallBase = true;

        //    command.With(context);
        //    command.OrganizationId = context.Organization.Id;
        //    command.EntityMetaData = entityMetadata;
        //    command.Workflow = workflow.Object;
        //    context.Workflow = command.Workflow;

        //    // Act
        //    var result = actor.ProcessWorkflowStep(context, command);
        //    Assert.Equal(WorkflowStepResult.Repeat.SaveResult, result.SaveResult);

        //    result = actor.ProcessWorkflowStep(context, command);
        //    Assert.Equal(WorkflowStepResult.Success.SaveResult, result.SaveResult);
        //    context.Workflow.AddStepResult(result);

        //    context.Workflow.ProcessStepResult(context, command);

        //    // Assert
        //    this.testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

        //    Assert.True(clues.Count > 0);
        //}

        [Fact(Skip = "GitHub Issue 829 - ref https://github.com/CluedIn-io/CluedIn/issues/829")]
        public void TestInvalidUser()
        {
            // Arrange
            this.testContext = new TestContext();
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.Email, "john.doe@example.org");
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.PhoneNumber, "88888888");
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.HomeAddressStreetName, "Hill Road");
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.HomeAddressStreetNumber, "6");
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.HomeAddressCity, "Atlantis");
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.HomeAddressZipCode, "666");
            IEntityMetadata entityMetadata = new EntityMetadataPart()
            {
                Name = "John Doe",
                EntityType = EntityType.Infrastructure.User,
                Properties = properties.Properties
            };

            var externalSearchProvider = new Mock<NameApiExternalSearchProvider>(MockBehavior.Loose);
            var clues = new List<CompressedClue>();

            externalSearchProvider.CallBase = true;

            this.testContext.ProcessingHub.Setup(h => h.SendCommand(It.IsAny<ProcessClueCommand>())).Callback<IProcessingCommand>(c => clues.Add(((ProcessClueCommand)c).Clue));

            this.testContext.Container.Register(Component.For<IExternalSearchProvider>().UsingFactoryMethod(() => externalSearchProvider.Object));
            var context = this.testContext.Context.ToProcessingContext();
            var command = new ExternalSearchCommand();
            var actor = new ExternalSearchProcessingAccessor(context.ApplicationContext);
            var workflow = new Mock<Workflow>(MockBehavior.Loose, context, new EmptyWorkflowTemplate<ExternalSearchCommand>());

            workflow.CallBase = true;

            command.With(context);
            command.OrganizationId = context.Organization.Id;
            command.EntityMetaData = entityMetadata;
            command.Workflow = workflow.Object;
            context.Workflow = command.Workflow;

            // Act
            var result = actor.ProcessWorkflowStep(context, command);
            Assert.Equal(WorkflowStepResult.Repeat.SaveResult, result.SaveResult);

            result = actor.ProcessWorkflowStep(context, command);
            Assert.Equal(WorkflowStepResult.Success.SaveResult, result.SaveResult);
            context.Workflow.AddStepResult(result);

            context.Workflow.ProcessStepResult(context, command);

            // Assert
            this.testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.True(clues.Count > 0);
        }

        public void Dispose()
        {
            this.testContext?.Dispose();
        }
    }
}
