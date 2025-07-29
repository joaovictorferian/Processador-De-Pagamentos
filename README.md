# 🧾 Processador de Pagamentos

Sistema de processamento de pagamentos assíncrono, utilizando **Clean Architecture**, **MongoDB**, **RabbitMQ** e **.NET 7+**, com mensagens em fila para confirmação de pagamentos.

## 📦 Tecnologias Utilizadas

- ✅ ASP.NET Core
- ✅ Clean Architecture
- ✅ MongoDB
- ✅ RabbitMQ
- ✅ Docker / Docker Compose

## 🧠 Funcionalidades

- Criação de novos pagamentos com status "Pending"
- Envio de mensagens para uma fila RabbitMQ (`payment-confirmation`)
- Consumidor que escuta a fila e confirma pagamentos recebidos
- Persistência dos pagamentos no MongoDB
- Separação clara entre camadas: Domain, Application, Infrastructure, API

## 🚀 Como executar o projeto

### Pré-requisitos

- [.NET 7+ SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)
- Visual Studio ou VS Code (opcional)

### 1. Subir MongoDB e RabbitMQ com Docker

```
docker-compose up -d
```

- RabbitMQ Admin: http://localhost:15672 (usuário: guest / senha: guest)
- MongoDB: `mongodb://localhost:27017`

### 2. Rodar a aplicação

No terminal, dentro da pasta `API/`:

```
dotnet run
```

A API ficará disponível em:

```
http://localhost:5000
```

## 📬 Endpoints disponíveis

### Criar pagamento

```http
POST /api/payment
Content-Type: application/json

{
  "amount": 99.90
}
```

## 🧪 Observações técnicas

- `PaymentMessage` é usado para envio na fila
- `SubscribeAsync` consome mensagens do RabbitMQ e atualiza o pagamento no MongoDB
- O consumo da fila começa automaticamente no `Program.cs`

## ✍️ Autor

Desenvolvido por **João Victor Canello Ferian** — projeto acadêmico e de estudo com Clean Architecture.
