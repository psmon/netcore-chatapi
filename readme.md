# ChatAPI

���� : .net core ChatAPI

Tech Concept : 
- .Net WebSocket
- Akka.net (���� �޽�¡ �����, �ڵ庯�� �ּ�ȭ�� �л�ó�� Ȯ���ϱ����ؼ�)
- Domain Driven Design (Eric J Evans)

Lib :
- Install-Package Microsoft.AspNetCore.WebSockets -Version 2.2.1
- Install-Package Akka.Cluster -Version 1.3.13
- Install-Package Akka.TestKit -Version 1.3.13


Test :
- ������ �׽�Ʈ : http://localhost:5000
- Rest�� ������ ���� : http://localhost:5000/swagger


links : �� ������Ʈ�� ���� �����...
- https://docs.microsoft.com/en-us/aspnet/core/fundamentals/websockets?view=aspnetcore-2.2
- https://havret.io/akka-net-asp-net-core
- https://radu-matei.com/blog/aspnet-core-websockets-middleware/
- https://github.com/Havret/akka-net-asp-net-core/tree/master/src/Bookstore