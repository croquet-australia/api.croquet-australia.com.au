Feature: Emails to 2017 GC U21 Worlds EOI

Scenario Outline: happy path
    Given tournament slug is '2017/gc/u21-worlds-eoi'
	And dateOfBirth is '<dateOfBirth>'
    When the entry is submitted
    Then an email using '<emailTemplate>' template is sent to the player

	Examples: 
		| dateOfBirth | emailTemplate              |
		| 1 Jan 1996  | 2017 GC U21 Worlds EOI.txt |
		| 1 Jan 1997  | 2017 GC U21 Worlds EOI.txt |
		| 23 Sep 1998 | 2017 GC U21 Worlds EOI.txt |
		| 24 Sep 1998 | 2017 GC U21 Worlds EOI.txt |
		| 25 Sep 1998 | 2017 GC U21 Worlds EOI.txt |
		| 1 Jan 1999  | 2017 GC U21 Worlds EOI.txt |

Scenario Outline: Player is too old
    Given tournament slug is '2017/gc/u21-worlds-eoi'
	And dateOfBirth is '<dateOfBirth>'
    When the entry is submitted
    Then the player should not be sent an email

	Examples: 
		| dateOfBirth |
		| 31 Jan 1995 |
