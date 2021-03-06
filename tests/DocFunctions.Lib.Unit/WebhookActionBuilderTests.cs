﻿using DocFunctions.Lib.Builders;
using DocFunctions.Lib.Models.Audit;
using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Unit.Builders;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocFunctions.Lib.Unit
{
    public class WebhookActionBuilderTests
    {
        [Fact]
        public void AddedFileCreatesNewBlogAction()
        {
            // Arrange
            var actionBuilder = new Mock<IActionBuilder>();
            var sut = new WebhookActionBuilder(actionBuilder.Object, new AuditTree());
            var webhookData = new WebhookDataBuilder().Build();

            // Act
            sut.Process(webhookData.Commits);

            // Assert
            actionBuilder.Verify(m => m.NewBlog(It.IsAny<Added>()), Times.Once);
            actionBuilder.Verify(m => m.Build(), Times.Once);
        }

        [Fact]
        public void AddedImageCreatesNewImageAction()
        {
            // Arrange
            var actionBuilder = new Mock<IActionBuilder>();
            var sut = new WebhookActionBuilder(actionBuilder.Object, new AuditTree());
            var webhookData = new WebhookDataBuilder().Build();

            // Act
            sut.Process(webhookData.Commits);

            // Assert
            actionBuilder.Verify(m => m.NewImage(It.IsAny<Added>()), Times.Once);
            actionBuilder.Verify(m => m.Build(), Times.Once);
        }

        [Fact]
        public void RemovedFileCreatesDeleteBlogAction()
        {
            // Arrange
            var actionBuilder = new Mock<IActionBuilder>();
            var sut = new WebhookActionBuilder(actionBuilder.Object, new AuditTree());
            var webhookData = new WebhookDataBuilder().Build();

            // Act
            sut.Process(webhookData.Commits);

            // Assert
            actionBuilder.Verify(m => m.DeleteBlog(It.IsAny<Removed>()), Times.Once);
            actionBuilder.Verify(m => m.Build());
        }

        [Fact]
        public void RemoveImageCreatesDeleteImageAction()
        {
            // Arrange
            var actionBuilder = new Mock<IActionBuilder>();
            var sut = new WebhookActionBuilder(actionBuilder.Object, new AuditTree());
            var webhookData = new WebhookDataBuilder().Build();

            // Act
            sut.Process(webhookData.Commits);

            // Assert
            actionBuilder.Verify(m => m.DeleteImage(It.IsAny<Removed>()), Times.Once);
            actionBuilder.Verify(m => m.Build());
        }

        [Fact]
        public void ModifiedFileCreatesModifyBlogAction()
        {
            // Arrange
            var actionBuilder = new Mock<IActionBuilder>();
            var sut = new WebhookActionBuilder(actionBuilder.Object, new AuditTree());
            var webhookData = new WebhookDataBuilder().Build();

            // Act
            sut.Process(webhookData.Commits);

            // Assert
            actionBuilder.Verify(m => m.ModifyBlog(It.IsAny<Modified>()), Times.Once);
            actionBuilder.Verify(m => m.Build());
        }

        [Fact]
        public void ModifiedImageCreatesModifyImageAction()
        {
            // Arrange
            var actionBuilder = new Mock<IActionBuilder>();
            var sut = new WebhookActionBuilder(actionBuilder.Object, new AuditTree());
            var webhookData = new WebhookDataBuilder().Build();

            // Act
            sut.Process(webhookData.Commits);

            // Assert
            actionBuilder.Verify(m => m.ModifyImage(It.IsAny<Modified>()), Times.Once);
            actionBuilder.Verify(m => m.Build());
        }

    }
}
