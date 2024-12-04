# Arquitetura Hexagonal - ApiSales

Este projeto segue o modelo de Arquitetura Hexagonal, implementado com **.NET 8**, promovendo modularidade e desacoplamento entre as camadas.

## Tecnologias e Implementações

- **Camada de Aplicação**: Gerencia fluxos e orquestra as interações entre domínios.
- **Domínio**: Contém as regras de negócios e as entidades principais.
- **Adaptação (Ports e Adapters)**:
  - Repositórios implementados com **Entity Framework** e suporte a **migrations**.
  - Configuração de banco de dados PostgreSQL.
- **Validações**: Utilização do **FluentValidation** para validações consistentes.
- **Eventos e Logs**:
  - Disparo de eventos com integridade garantida.
  - Log centralizado utilizando **Serilog**.
- **Middleware**: Centralização do tratamento de exceções para respostas padronizadas.
- **Seed de Dados**: Popular automaticamente dados básicos para inicialização.

## Comando para Migrations

```bash
dotnet ef migrations add NOMEMIGRATION --project ..\src\Sales.Data\Sales.Data.csproj --startup-project .\Sales.WebApi.csproj
```

## Exemplo de Payload para Integração

```json
{
  "idSale": 1,
  "numberSale": "123456",
  "dateSale": "2024-12-03T06:26:04.081Z",
  "client": "Client ",
  "idClient": 1,
  "valueTotal": 100.50,
  "branch": "Branch Central",
  "idBranch": 1,
  "statusSale": "Não Cancelado",
  "itens": [
    {
      "product": "Product  1",
      "quantity": 2,
      "priceUnit": 50.00,
      "discount": 0,
      "valueTotalItem": 100.00,
      "itemSaleId": 1,
      "productId": 1
    }
  ]
}
```

Este projeto foi desenvolvido com foco na escalabilidade, permitindo uma evolução clara e eficiente.