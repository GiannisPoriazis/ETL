# ACS Assignment Project - ETL & Serialization Pipeline

This repository contains a solution for the coding challenge. The application is built using **.NET 10** and executes a high-performance, unidirectional **ETL (Extract, Transform, Load)** pipeline alongside its structural inverse (Serialization).

## 🚀 Architectural Blueprint & Design Patterns

The project has been architected according to **Clean Architecture** and **SOLID Principles** to ensure absolute decoupling, testability, and adherence to the Open-Closed Principle.

### Core Architecture Highlights:

- **Single Responsibility Principle (SRP):** Every component has a singular reason to change. File I/O operations, business metric reporting, core stream processing, and text block mapping are completely isolated from one another.
- **Open-Closed Principle (OCP):** Introducing a new record type (e.g., `VENDOR`) requires zero modifications to the orchestrator or existing parsers. You simply implement the `ILineParser` interface, and it automatically hooks into the pipeline via Dependency Injection.
- **Dependency Inversion Principle (DIP):** High-level business processing components depend entirely on abstractions (`IFileSystem`, `ILineParser`, `IReportService`), making the application entirely storage-agnostic.
- **Memory Efficiency:** File streams are evaluated lazily line-by-line using C# Iterators (`IEnumerable` streaming), maintaining an O(1) memory allocation footprint ideal for massive dataset ingestion.
- **Hierarchical Domain Model:** To prevent orphaned transactions and maintain explicit entity mapping, `TransactionRecord` instances are strictly modeled as children of their parent `CustomerRecord`.

---

## 📂 Project Structure

```
📦 AcsExam
┣ 📂 AcsExam.Core # Pure Domain Logic & Architecture Contracts
┃ ┣ 📂 Extensions # Service Collection DI Registrations
┃ ┣ 📂 Interfaces # Abstraction Layer (IFileSystem, ILineParser, etc.)
┃ ┣ 📂 Models # Hierarchical Domain & Metadata Objects
┃ ┣ 📂 Parsers # Individual Stateless Line Resolvers (OCP Strategy)
┃ ┣ 📂 Services # File Processor Orchestrator & Console Presenter
┣ ┣ 📜 ACS_EXAM_READ_FILE.dat # Source Challenge Input File
┣ ┗ 📜 Program.cs # Modern DI Bootstrapper & Entry Point
┗ 📂 AcsExam.Tests # xUnit Test Suite (TDD Behavior & In-Memory Mocks)
```

## 🧪 Testing Strategy (TDD Approach)

The solution was written following Test-Driven Development (TDD) principles. The domain layer remains 100% testable without touching physical drives by isolating dependencies through mocked wrappers.
Unit Tests: Utilize Moq to safely stub storage interfaces (IFileSystem) and supply in-memory string matrices, allowing microsecond test execution and robust edge-case validation (e.g., checksum validation failures).
To execute the test suite, navigate to the solution folder and run:

```
dotnet test
```

## ⚙️ Inversion of Control (IoC) Service Scope

The individual string segment mapping units are registered within the Microsoft DI Framework engine as Singleton lifetimes. Because they are entirely stateless, they are implicitly thread-safe. This maximizes memory efficiency and mitigates overall Garbage Collection (GC) pressure inside high-throughput transaction pipelines.
