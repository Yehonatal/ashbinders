# Documentation Standards

## 1. Core Rule: Zero Fluff, Zero Emojis, Zero AI Slop
All documentation, architectural records, pull request descriptions, code comments, and technical specifications must adhere strictly to professional engineering standards:

- **No Emojis**: Never use emojis anywhere in code, markdown documentation, scripts, commit messages, or terminal outputs.
- **No Conversational AI Slop**: Avoid filler phrases, patronizing introductions, buzzwords, generic motivational platitudes, or robotic intros ("In this section, we will delve into...").
- **Direct and Technical**: State facts, constraints, interfaces, mathematical models, and architectural decisions directly and concisely.
- **High Information Density**: Prioritize tables, interface contracts, precise types, algorithmic complexity, and concrete code examples over explanatory prose.

## 2. Code Comments Standard
- Comments must explain **why** an approach was taken, not repeat what the code obviously does.
- Public APIs, interfaces, and exported properties must have clear, concise doc comments.
- Do not leave dead code, temporary debug logs, or commented-out blocks in version control.
