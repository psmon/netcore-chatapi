# ChatAPI

목적 : .net core ChatAPI

Tech Concept : 
- .Net WebSocket
- Akka.net (로컬 메시징 기능을, 코드변경 최소화로 분산처리 확장하기위해서)
- Domain Driven Design (Eric J Evans)

Lib :
- Install-Package Microsoft.AspNetCore.WebSockets -Version 2.2.1
- Install-Package Akka.Cluster -Version 1.3.13
- Install-Package Akka.TestKit -Version 1.3.13


Test :
- 웹소켓 테스트 : http://localhost:5000
- Rest와 웹소켓 연동 : http://localhost:5000/swagger


links : 이 프로젝트에 사용된 개념들...
- https://docs.microsoft.com/en-us/aspnet/core/fundamentals/websockets?view=aspnetcore-2.2
- https://havret.io/akka-net-asp-net-core
- https://radu-matei.com/blog/aspnet-core-websockets-middleware/
- https://github.com/Havret/akka-net-asp-net-core/tree/master/src/Bookstore