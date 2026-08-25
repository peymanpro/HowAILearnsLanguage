@'
# HowAILearnsLanguage

**How can a machine learn language representations from sentences?**

`HowAILearnsLanguage` is a from-scratch .NET 8 / C# implementation of a tiny language-learning system.

The project starts with randomly initialized word representations and learns them from sentences through:

```text
Training Data
     ↓
Context Prediction
     ↓
Forward Pass
     ↓
Softmax
     ↓
Cross-Entropy Loss
     ↓
Backpropagation
     ↓
Gradient Descent
     ↓
Updated Embeddings

The goal is not to build an LLM.

The goal is to make the learning mechanism visible.

The Core Question

A machine does not begin with a semantic concept such as:

cat
dog
milk

It begins with parameters.

This project demonstrates how those parameters can be adjusted by observing language.

Random Embeddings
       ↓
Language Data
       ↓
Prediction Error
       ↓
Gradients
       ↓
Parameter Updates
       ↓
Learned Representations
What the Model Learns

The current model uses a simple context-prediction objective.

For a sentence such as:

The cat drinks milk.

with a context radius of 1, training examples are created from neighboring words:

target      context
-------------------
the         cat
cat         the
cat         drinks
drinks      cat
drinks      milk
milk        drinks

For a training example:

target = cat
context = drinks

the model computes:

cat embedding
      ↓
dot product with output embeddings
      ↓
scores
      ↓
softmax
      ↓
P(context | target)

The loss is cross-entropy:

L = -log(P(correct context))

Backpropagation then computes the gradients, and gradient descent updates the embeddings:

embedding ← embedding - learningRate × gradient

The same procedure is repeated over many training examples and epochs.

Architecture

The project intentionally uses a small number of components.

HowAILearnsLanguage
│
├── Domain
│   ├── Vocabulary
│   ├── TrainingExample
│   ├── TrainingCorpus
│   ├── TrainingData
│   └── Tokenizer
│
├── Mathematics
│   └── Vector
│
└── Learning
    ├── LanguageModel
    ├── Prediction
    ├── ILossFunction
    ├── CrossEntropyLoss
    ├── LanguageGradients
    ├── Backpropagator
    ├── IOptimizer
    ├── GradientDescentOptimizer
    ├── Trainer
    ├── Evaluator
    ├── EmbeddingSnapshot
    ├── EmbeddingEvaluator
    └── LanguageLearningExperiment

Program.cs is intentionally kept as the composition root and presentation layer.

The learning logic is not placed inside Program.cs.

Why the Interfaces Exist

The project avoids artificial abstraction.

Only boundaries that represent an actual policy or responsibility are abstracted.

For example:

ILossFunction
     ↓
CrossEntropyLoss

IOptimizer
     ↓
GradientDescentOptimizer

This makes it possible to replace the learning policy without changing the trainer or language model.

For example, another optimizer could later implement:

IOptimizer

without changing the training orchestration.

The project intentionally avoids creating interfaces for every class.

Training Experiment

The current starter corpus contains:

12 training sentences
13 vocabulary tokens

Training uses:

Embedding dimension: 8
Context radius:      1
Epochs:              50
Learning rate:       0.1

A representative training run produced:

Initial loss: 2.536511
Final loss:   1.460828

The reduction in loss demonstrates that the model successfully optimized its training objective.

Evaluation

Training performance alone is not enough.

A separate evaluation corpus is used to test the trained model on sentences that were not used during training.

Current evaluation:

Examples: 44
Correct:  20
Accuracy: 45.45%
Avg Loss: 1.333968

This number should not be interpreted as "45.45% language understanding".

It is the top-1 accuracy of the current context-prediction task on the small evaluation corpus.

The evaluation exists to distinguish:

Training optimization
        from
Generalization
What Did the Model Actually Learn?

The project also inspects the learned embedding space.

Before training:

cat ↔ kitten   -0.3224
dog ↔ puppy    -0.4283
cat ↔ dog      -0.2369
cat ↔ milk      0.4847

After training:

cat ↔ kitten    0.9939
dog ↔ puppy     0.9896
cat ↔ dog       0.9953
cat ↔ milk      0.4556

This is an important result.

The embeddings began as randomly initialized parameters and changed substantially during training.

The model therefore did not receive semantic labels such as:

cat = animal
kitten = young cat

Instead, it learned from the distribution of words in context.

An Important Limitation

The result:

cat ↔ dog = 0.9953

is intentionally informative.

The model considers cat and dog almost identical because the training objective rewards similar contextual behavior.

This demonstrates a fundamental limitation of the current architecture:

Similar distributional context does not necessarily mean identical semantic meaning.

The model captures a useful statistical structure, but it does not provide full language understanding.

This limitation is part of the result, not something hidden from the experiment.

What This Project Does Not Implement

This repository intentionally does not implement:

Transformer architecture
Self-attention
Multi-head attention
Large language models
Recurrent neural networks
Large-scale datasets
Pretrained embeddings
External NLP or ML frameworks

Those mechanisms belong to later stages of the learning path.

The current project focuses on one question:

How can language representations be learned from sentences using gradient-based learning?

Technology
C#
.NET 8
xUnit
Git

The implementation uses no ML or NLP framework.

Core vector mathematics, prediction, loss, backpropagation, optimization, training, and evaluation are implemented directly in C#.

Run the Project

Clone the repository and enter the project:

git clone <repository-url>
cd HowAILearnsLanguage

Build:

dotnet build

Run tests:

dotnet test

Run the experiment:

dotnet run --project ./HowAILearnsLanguage

The console output shows:

Training
Evaluation
Learned Embeddings
Development Flow

The implementation follows a deliberate progression:

Mathematical Primitive
        ↓
Domain Model
        ↓
Forward Pass
        ↓
Loss
        ↓
Backpropagation
        ↓
Optimizer
        ↓
Training Loop
        ↓
Evaluation
        ↓
Learned Representation

Every major step is covered by automated tests before the next learning mechanism is introduced.

Test Coverage

The test suite currently covers:

vector mathematics
vocabulary management
training examples
tokenization
model prediction
softmax probabilities
cross-entropy loss
backpropagation
gradient descent
training steps
corpus handling
training example generation
training loops
evaluation
embedding snapshots
before/after embedding comparison
end-to-end language learning experiments

The current test suite contains 55 passing tests.

Learning Path

This project sits between mathematical deep-learning foundations and more advanced language-model architectures.

HowDeepLearningWorks
        ↓
HowAIRepresentsLanguage
        ↓
HowAILearnsLanguage
        ↓
HowAttentionWorks
        ↓
HowTransformersWork

The distinction is intentional:

HowDeepLearningWorks
→ How does a neural network learn?

HowAIRepresentsLanguage
→ How can language be represented numerically?

HowAILearnsLanguage
→ How can those representations be learned from language data?

HowAttentionWorks
→ How can a model learn which context matters?

HowTransformersWork
→ How are these ideas assembled into a Transformer?
Final Takeaway

The central lesson of this project is simple:

Language
   ↓
Training Examples
   ↓
Prediction
   ↓
Error
   ↓
Gradients
   ↓
Parameter Updates
   ↓
Learned Representation

A machine does not need to be given an explicit definition of a word to begin learning useful structure about it.

It can adjust numerical representations indirectly by repeatedly trying to predict linguistic context and correcting its errors.

At the same time, the experiment shows the limitation clearly:

Learning contextual statistics is not the same as understanding language.

That boundary is exactly where more advanced architectures begin.
'@ | Set-Content -Encoding UTF8 .\README.md


### 2. ADR

```powershell
New-Item -ItemType Directory -Force .\docs\architecture\adr | Out-Null

@'
# ADR-001: Learn Language Representations Through Context Prediction

## Status

Accepted

## Context

The purpose of `HowAILearnsLanguage` is to demonstrate how numerical language representations can be learned from sentences.

The project must remain small enough that the entire learning mechanism can be inspected in ordinary C# code.

The implementation therefore avoids external machine-learning and NLP frameworks.

## Decision

The first learning objective will be context prediction.

A target token is used to predict neighboring context tokens:

```text
target token
    ↓
input embedding
    ↓
dot products
    ↓
softmax
    ↓
context probability distribution

The learning objective is cross-entropy loss.

Gradients are computed analytically through the softmax/cross-entropy path and applied using gradient descent.

The resulting pipeline is:

Training Example
      ↓
Forward Pass
      ↓
Prediction
      ↓
Cross-Entropy Loss
      ↓
Backpropagation
      ↓
Gradient Descent
      ↓
Updated Embeddings
Architectural Decisions

The model owns its parameters and forward pass.

Backpropagator computes gradients without mutating the model.

IOptimizer represents the parameter-update policy.

Trainer orchestrates the learning loop.

Evaluator measures model behavior on evaluation examples.

These boundaries are intentionally small.

Interfaces are introduced only where they represent a replaceable policy, such as loss calculation or optimization.

Consequences

The implementation is easy to inspect and test.

The learning process is explicit rather than hidden inside a machine-learning framework.

The architecture can later support other loss functions or optimizers without rewriting the training orchestration.

The simplicity also creates known limitations:

the model has a small context-prediction objective
the corpus is tiny
word order is represented only through local context examples
similar distributional contexts can collapse distinct semantic concepts
the system is not a language model in the modern LLM sense

These limitations are intentional and are part of the experiment.

Scope Boundary

Attention, Transformers, large corpora, and large-scale neural language modeling are outside the scope of this repository.

They should be explored separately after the current learning mechanism is understood.
'@ | Set-Content -Encoding UTF8 .\docs\architecture\adr\ADR-001-context-prediction-learning.md