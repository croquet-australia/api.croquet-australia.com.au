Feature: Send Email via SendInBlue

Scenario: Zero attachments
	Given a valid EmailMessage
	When EmailService.SendAsync(EmailMessage emailMessage) is called
	Then the email is sent
	And messageId is returned

Scenario: One attachment
	Given a valid EmailMessage with 1 attachment
	When EmailService.SendAsync(EmailMessage emailMessage) is called
	Then the email is sent
	And messageId is returned
