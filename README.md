# Virtual Pet Application

**FBLA Introduction to Programming (2025–2026)**

---

## Project Overview

This project is a **Virtual Pet** application created for the FBLA *Introduction to Programming* competitive event. The program allows a user to create, care for, and manage a virtual pet over time. The pet responds to user actions, tracks the cost of care, and **persists data across runs**.

The goal of this project is to demonstrate foundational programming concepts including variables, conditionals, loops, functions, data storage, and user input validation.

---

## Features

* Create and name a virtual pet
* Choose from **multiple pet types**
* Care actions:

  * Feed
  * Play
  * Rest
  * Clean
  * Vet / Health Check
* Pet reactions based on care (mood, health, happiness, etc.)
* **Cost of care tracking** with a running total
* **Persistent storage** (pet data is saved and loaded between runs)
* Input validation to prevent crashes
* Text-based console interface

---

## How to Run the Program

### Requirements

* .NET SDK 8.0 or newer
* Windows, macOS, or Linux

### Steps

1. Open a terminal or command prompt
2. Navigate to the project folder
3. Run the following command:

   ```
   dotnet run
   ```
4. Follow the on-screen menu instructions

---

## How Data Is Stored

Pet data is saved locally using a structured data file (JSON format). The following information is stored:

* Pet name and type
* Pet stats (hunger, energy, cleanliness, happiness, health)
* Pet mood
* Total cost of care

When the program restarts, the saved file is loaded automatically so the user can continue caring for the same pet.

---

## Input Validation

The program validates user input by:

* Restricting menu choices to valid numeric ranges
* Preventing empty or overly long pet names
* Displaying friendly error messages when invalid input is entered

This ensures the program runs smoothly without crashing.

---

## Programming Concepts Demonstrated

* Variables and data types
* Conditional statements (`if / else`)
* Loops
* Functions / methods
* Classes and objects
* File input/output (data persistence)
* Error handling and validation
* Dependency injection for testability

---

## Testing & Quality Assurance

This project includes a **full automated test suite** to validate correctness, reliability, and edge cases.

### Testing Framework

* **xUnit** – primary unit testing framework
* **FluentAssertions** – readable, expressive assertions

### What Is Tested

* **Models** (Pet stat clamping, mood logic)
* **Input Handling** (numeric ranges, required text input)
* **Persistence** (save/load, missing or corrupted data)
* **Game Logic** (pet creation, care actions, cost tracking, save/reset flows)

### Testing Techniques Used

* Unit testing
* Fake / mock implementations for:

  * Console input/output
  * File storage
  * System clock (time)

This ensures tests are **deterministic**, repeatable, and do not rely on real user input or the file system.

### Running Tests

```
dotnet test
```

---

## Attribution & Resources

This project was created by **[Team Name]** for FBLA competition use only.

### Libraries & Tools

* **.NET / C# Standard Library**

  * Used for console input/output, file handling, and JSON serialization

### External Assets

* None

### AI Assistance

* AI tools were used as a learning aid to help explain programming concepts and assist with code structure.
* All final code was reviewed, tested, and understood by the team.

---

## Known Limitations / Future Improvements

* Add graphical user interface (GUI)
* Add more pet types and care actions
* Add achievements or rewards system
* Expand financial features (budget limits or earning money)

---

## Contact Information

For questions regarding this project:

**Team Name:** ____________________
**School:** _______________________
**Event:** FBLA Introduction to Programming

---

*This README is included to meet FBLA documentation requirements and demonstrate understanding of the program design and implementation.*
