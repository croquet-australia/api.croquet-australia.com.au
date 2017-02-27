Feature: Emails to 2017 Gateball Championships

Scenario Outline: happy path
    Given tournament slug is '2017/gb/championships'
    And paymentMethod is '<paymentMethod>'
    When the entry is submitted
    Then an email using '<emailTemplate>' template is sent to the player
	And the email should have attachment '<emailAttachment>'

	Examples:
		| paymentMethod | emailTemplate                   | emailAttachment                              |
		| EFT           | 2017 Gateball Championships.txt | 2017 Gateball Championships Team Details.pdf |
		| Cheque        | 2017 Gateball Championships.txt | 2017 Gateball Championships Team Details.pdf |

