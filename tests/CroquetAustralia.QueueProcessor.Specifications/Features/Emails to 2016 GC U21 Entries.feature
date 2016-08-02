Feature: Emails to 2016 GC U21 Entries

Scenario Outline: happy path
    Given tournament slug is '2016/gc/u21'
    And paymentMethod is '<paymentMethod>'
	And dateOfBirth is '<dateOfBirth>'
	And nonResident is '<nonResident>'
    When the entry is submitted
    Then an email using '<emailTemplate>' template is sent to the player
	And the email should have attachment '<emailAttachment>'

	Examples: 
		| paymentMethod | dateOfBirth | nonResident | emailTemplate                  | emailAttachment               |
		| EFT           | 1 Jan 1995  | false       | 18-21 and Australian.txt       |                               |
		| EFT           | 1 Jan 1996  | false       | 18-21 and Australian.txt       |                               |
		| EFT           | 1 Jan 1997  | false       | 18-21 and Australian.txt       |                               |
		| EFT           | 23 Sep 1998 | false       | 18-21 and Australian.txt       |                               |
		| EFT           | 24 Sep 1998 | false       | Under 18 and Australian.txt    | Under 18 Consent Form AUS.pdf |
		| EFT           | 25 Sep 1998 | false       | Under 18 and Australian.txt    | Under 18 Consent Form AUS.pdf |
		| EFT           | 1 Jan 1999  | false       | Under 18 and Australian.txt    | Under 18 Consent Form AUS.pdf |
		| Cash          | 1 Jan 1995  | true        | 18-21 and New Zealander.txt    |                               |
		| Cash          | 1 Jan 1996  | true        | 18-21 and New Zealander.txt    |                               |
		| Cash          | 1 Jan 1997  | true        | 18-21 and New Zealander.txt    |                               |
		| Cash          | 23 Sep 1998 | true        | 18-21 and New Zealander.txt    |                               |
		| Cash          | 24 Sep 1998 | true        | Under 18 and New Zealander.txt | Under 18 Consent Form NZ.pdf  |
		| Cash          | 25 Sep 1998 | true        | Under 18 and New Zealander.txt | Under 18 Consent Form NZ.pdf  |
		| Cash          | 1 Jan 1999  | true        | Under 18 and New Zealander.txt | Under 18 Consent Form NZ.pdf  |

Scenario Outline: Player is too old
    Given tournament slug is '2016/gc/u21'
    And paymentMethod is '<paymentMethod>'
	And dateOfBirth is '<dateOfBirth>'
	And nonResident is '<nonResident>'
    When the entry is submitted
    Then the player should not be sent an email

	Examples: 
		| paymentMethod | dateOfBirth | nonResident |
		| EFT           | 31 Jan 1994 | false       |
		| Cash          | 31 Jan 1994 | true        |
