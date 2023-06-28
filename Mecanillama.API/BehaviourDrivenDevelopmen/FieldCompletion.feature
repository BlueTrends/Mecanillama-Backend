Feature: Field Completion
    Scenario: Missing Required Field
        Given I am filling in the fields
        When I leave a required field empty
        And the user clicks the "Submit" button
        Then the system should not allow login

    Scenario: Successful Login
        Given I am filling in the fields
        When I complete all the required fields correctly
        And the user clicks the "Submit" button
        Then the system should allow login




        
        