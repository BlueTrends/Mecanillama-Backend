Feature: User Login
  Scenario: Successful Login
    Given I am on the login page
    And I enter my valid credentials
    When I click the "Login" button
    Then I should be logged in successfully
  Scenario: Invalid Credentials
    Given I am on the login page
    And I enter invalid credentials
    When I click the "Login" button
    Then I should see the message: "Invalid credentials. Please try again"
