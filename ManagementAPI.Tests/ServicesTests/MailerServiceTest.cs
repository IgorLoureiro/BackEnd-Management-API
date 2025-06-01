using ManagementAPI.Services;

namespace ManagementAPI.Tests.MailerServiceTests
{
    public class MailerServiceTests
    {
        public MailerServiceTests()
        {
            // Set environment variables needed by MailerService
            Environment.SetEnvironmentVariable("EMAIL_SENDER", "sender@test.com");
            Environment.SetEnvironmentVariable("EMAIL_SENDER_APP_PASSWORD", "password123");
            Environment.SetEnvironmentVariable("SMTP_SERVER", "smtp.testserver.com");
            Environment.SetEnvironmentVariable("SMTP_PORT", "587");
        }

        [Fact]
        public void GetSenderEmailAddress_ShouldReturnConfiguredEmail()
        {
            // Arrange
            var mailerService = new MailerService();

            // Act
            var email = mailerService.GetSenderEmailAddress();

            // Assert
            Assert.Equal("sender@test.com", email);
        }

        [Fact]
        public void GetSenderEmailPassword_ShouldReturnConfiguredPassword()
        {
            // Arrange
            var mailerService = new MailerService();

            // Act
            var password = mailerService.GetSenderEmailPassword();

            // Assert
            Assert.Equal("password123", password);
        }

        [Fact]
        public async Task SenderMail_ShouldSendEmailSuccessfully()
        {
            // Arrange
            var mailerService = new MailerService();

            var recipientName = "Test User";
            var recipientEmail = "testuser@test.com";
            var subject = "Test Subject";
            var message = "This is a test message.";

            // Act & Assert
            var exception = await Record.ExceptionAsync(() =>
                mailerService.SenderMail(recipientName, recipientEmail, subject, message));

            Assert.Null(exception);
        }
    }
}
