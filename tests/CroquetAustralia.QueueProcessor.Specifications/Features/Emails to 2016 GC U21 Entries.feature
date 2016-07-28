Feature: Emails to 2016 GC U21 Entries

Scenario Outline: happy path
    Given tournament slug is '2016/gc/u21'
    And paymentMethod is '<paymentMethod>'
	And yearOfBirth is '<yearOfBirth>'
	And nonResident is '<nonResident>'
    When the entry is submitted
    Then an email using '<emailTemplate>' template is sent to the player
	And the email should have attachment '<emailAttachment>'

	Examples: 
		| paymentMethod | yearOfBirth | nonResident | emailTemplate                  | emailAttachment               |
		| EFT           | 1995        | false       | 18-21 and Australian.txt       |                               |
		| EFT           | 1996        | false       | 18-21 and Australian.txt       |                               |
		| EFT           | 1997        | false       | 18-21 and Australian.txt       |                               |
		| EFT           | 1998        | false       | 18-21 and Australian.txt       |                               |
		| EFT           | 1999        | false       | Under 18 and Australian.txt    | Under 18 Consent Form AUS.pdf |
		| EFT           | 2000        | false       | Under 18 and Australian.txt    | Under 18 Consent Form AUS.pdf |
		| EFT           | 2001        | false       | Under 18 and Australian.txt    | Under 18 Consent Form AUS.pdf |
		| EFT           | 2002        | false       | Under 18 and Australian.txt    | Under 18 Consent Form AUS.pdf |
		| EFT           | 2003        | false       | Under 18 and Australian.txt    | Under 18 Consent Form AUS.pdf |
		| EFT           | 2004        | false       | Under 18 and Australian.txt    | Under 18 Consent Form AUS.pdf |
		| Cash          | 1995        | true        | 18-21 and New Zealander.txt    |                               |
		| Cash          | 1996        | true        | 18-21 and New Zealander.txt    |                               |
		| Cash          | 1997        | true        | 18-21 and New Zealander.txt    |                               |
		| Cash          | 1998        | true        | 18-21 and New Zealander.txt    |                               |
		| Cash          | 1999        | true        | Under 18 and New Zealander.txt | Under 18 Consent Form NZ.pdf  |
		| Cash          | 2000        | true        | Under 18 and New Zealander.txt | Under 18 Consent Form NZ.pdf  |
		| Cash          | 2001        | true        | Under 18 and New Zealander.txt | Under 18 Consent Form NZ.pdf  |
		| Cash          | 2002        | true        | Under 18 and New Zealander.txt | Under 18 Consent Form NZ.pdf  |
		| Cash          | 2003        | true        | Under 18 and New Zealander.txt | Under 18 Consent Form NZ.pdf  |
		| Cash          | 2004        | true        | Under 18 and New Zealander.txt | Under 18 Consent Form NZ.pdf  |

Scenario Outline: Player is too old
    Given tournament slug is '2016/gc/u21'
    And paymentMethod is '<paymentMethod>'
	And yearOfBirth is '<yearOfBirth>'
	And nonResident is '<nonResident>'
    When the entry is submitted
    Then the player should not be sent an email

	Examples: 
		| paymentMethod | yearOfBirth | nonResident |
		| EFT           | 1994        | false       |
		| Cash          | 1944        | true        |
