Feature: Generate
    As Generator user
    I want to get generated content
    So that I can improve my development speed

Scenario Outline: generate code
    Given I have source code in file <input>
    When I ask to generate
    Then the result should be like in file <output>

	Examples: 
	| input                    | output                    |
	| AutogenCommand.in.txt    | AutogenCommand.out.txt    |
	| AutogenDP.in.txt         | AutogenDP.out.txt         |
	| MisleadInComments.in.txt | MisleadInComments.out.txt |
	| MixedTypeAutogen.in.txt  | MixedTypeAutogen.out.txt  |
	| MultipleClass.in.txt     | MultipleClass.out.txt     |
	| NoPartialClass.in.txt    | NoPartialClass.out.txt    |
	| StandardAutogen.in.txt   | StandardAutogen.out.txt   |
