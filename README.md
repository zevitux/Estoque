# 📦 EstoqueGerenciamento — Sistema de Gerenciamento de Estoque e Pedidos

Este projeto é uma **ASP.NET Core Web API** para gerenciamento de estoque, pedidos, produtos, fornecedores e itens de pedidos. Ele foi criado como uma base para aplicações de controle de estoque, permitindo operações CRUD, rastreamento de pedidos por fornecedor e geração de relatórios.

## ✨ Funcionalidades
- Cadastro e gerenciamento de **produtos**
- Registro de **fornecedores**
- Criação e acompanhamento de **pedidos** e seus itens
- Cálculo automático de totais de pedidos
- Organização modular usando **Repositories**, **Services** e **DTOs**
- **Logs** detalhados com Microsoft.Extensions.Logging
- Pronto para extensões como geração de relatórios em PDF, exportação e filtros avançados
  
---

## 🚀 Como rodar o projeto
1. Clone este repositório:
   ```bash
   git clone https://github.com/zevitux/Estoque.git
   ```

2. Configure a connection string no `appsettings.json`.

3. Execute as migrations:
   ```bash
   dotnet ef database update
   ```

4. Rode o projeto:
   ```bash
   dotnet run
   ```

5. Acesse a documentação Scalar em:
   ```
   https://localhost:{porta}/scalar
   ```
---
## 🤝 Contribuição
Este é um projeto de estudos e está **aberto a críticas e sugestões**.  
Sinta-se à vontade para abrir issues, enviar pull requests ou apenas deixar um comentário com feedback!
