# CSharp RAG Assistant

A simple .NET console application that demonstrates a retrieval-augmented generation (RAG) workflow using C#, local documents, and Ollama.

This project is based on the "Csharp AI Agent ME Chapter 12" sample and shows how to:

- load a knowledge base from text documents
- split and index document content
- retrieve relevant chunks based on a user question
- send the retrieved context to a local LLM via Ollama
- answer with grounded responses derived from the source documents only

## Features

- Console-based chat experience
- Keyword-based document retrieval
- Local AI inference using Ollama
- Source-aware answers that reference the retrieved documents
- Sample policy documents for refund, shipping, support, and warranty questions

## Project structure

- `Program.cs` - main application loop and user interaction
- `CSharpRagAssistant.csproj` - .NET project configuration
- `Documents/` - sample knowledge base files
- `Knowledge/` - knowledge loading logic
- `Retrieval/` - retrieval implementation
- `Services/` - AI service integration
- `Models/` - data models used by the app

## Prerequisites

- .NET 10 SDK
- Ollama installed and running locally
- A compatible model available in Ollama, configured in the app as `qwen3.5:9b`

Install Ollama from the official site, then pull the model:

```bash
ollama pull qwen3.5:9b
```

Make sure the local API is available at:

```text
http://localhost:11434/
```

## Run the app

```bash
dotnet restore
dotnet run
```

Once running, you can ask questions such as:

- What is the refund policy?
- Does the warranty cover accidental damage?
- How long does shipping take?
- Who can help with a support issue?

The app will retrieve matching document chunks and ask the model to answer using only that context.

## How it works

1. The application loads policy documents from the `Documents` folder.
2. It creates keyword-based document chunks for search.
3. A user question is matched against the loaded content.
4. The most relevant chunks are passed to Ollama.
5. The LLM answers using only the retrieved document data.

This creates a lightweight RAG pattern without requiring a database or external hosted API.

## Sample documents

The project includes sample policy documents for:

- refund policy
- shipping policy
- support policy
- warranty policy

These serve as a small knowledge base for demo and learning purposes.

## Notes

- The project intentionally uses a simple retrieval approach rather than a production search stack.
- The app is designed for local experimentation and learning.
- If Ollama is not running or the model is missing, the app will return an error when it tries to generate a response.

## License

This repository does not currently specify a license file.
