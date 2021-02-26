# Authentication-API
Development of an authentication API with .NET Core 5, EntityFramework with SQL Server and JwtBearer scheme

This project was developed with the objective of improving my knowledge in API projects by applying EntityFramework-Core with the .NET Core authentication system (JWT Bearer). Three endpoints were implemented in order to insert a registration form for new users, an endpoint for login (creating a token for a username and password) and another with access using an authentication token. 

I also used a test project to improve my skills in unit and behavior tests.

I cared about architecture in such a way as to separate folders into responsibilities, using onion architecture principles. I also applied concepts of SOLID and dependency injection. I was concerned with CleanCode and the scalability of the project for future implementations. I also used market standards for organization, focusing a lot on the security system (project focus).

## REST API

### Register a new user

`PUT /api/Auth/register`

    curl --location --request POST 'http://localhost:5000/api/Auth/register' \ 
    --header 'Content-Type: application/json' \ 
    --data-raw '{
      "firstName": "firstname_user",
      "lastName": "lastname_user",
      "username": "username_user",
      "password": "password_user"
    }'

#### Response is success case

    Status: 200 OK
    
    {
    "user": {
        "id": {{$guid}},
        "firstName": "firstname_user",
        "lastName": "lastname_user",
        "username": "username_user"
    },
    "token": "token.123.abc"
    }

* The username must be unique and the fields cannot be null.

### Authenticate with a user

`POST /api/Auth/login`

    curl --location --request POST 'http://localhost:5000/api/Auth/login' \
    --header 'Content-Type: application/json' \
    --data-raw '{
      "username": "username_user",
      "password": "password_user"
    }'

#### Response is success case

    Status: 200 OK
    
    {
    "user": {
        "id": {{$guid}},
        "firstName": "firstname_user",
        "lastName": "lastname_user",
        "username": "username_user"
    },
    "token": "token.123.abc"
    }
    
### Acess with a valid token 

`GET /api/Auth`

    curl --location --request GET 'http://localhost:5000/api/Auth' \
    --header 'Authorization: Bearer token.123.abc'
    
#### Response is success case

    Status: 200 OK
    
    {
      "message": "User authenticated",
      "username": "username_user",
      "id": {{$guid}},
      "status": "Success"
    }

---

I implemented several input checks, with HTTP returns consistent with the REST API standard. I also practiced injecting information into the token through Claims, in order to be able to retrieve user information later.

In this application I took care to encrypt users passwords in order to persist Hash and Salt data from the string. The verification is done before performing the authentication in such a way as to use the class 'HashService.cs'

    private static PasswordHash CreatePasswordHash(string password)
    {
      byte[] passwordHash, passwordSalt;
      if (password == null) throw new ArgumentNullException("password");
      if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }

      return new PasswordHash { passwordHash = passwordHash, passwordSalt = passwordSalt };
    }
    
## Tests
    
 To implement the tests I used the xUnit library, applying the AutoFixture, FluentAssertions and Moq helper packages. 
 
 I was able to learn a lot about how it is possible to "mock" methods, develop tests in SQL databases and apply libraries that assist in the reading and development of the checks, in addition to creating temporary objects for testing. A test example can be seen below.
 
    [Fact]
    public async Task GivenAValidUser_ShouldAuthenticateAsync()
    {
      var password = "teste";
      var passwordEncrypted = HashServices.CreatePasswordEncrypted(password);

       var userModel = _fixture.Build<AuthenticateUser>()
           .With(u => u.Password, password)
           .Create();

       var user = _fixture.Build<User>()
           .With(u => u.PasswordHash, passwordEncrypted.passwordHash)
           .With(u => u.PasswordSalt, passwordEncrypted.passwordSalt)
           .Create();

       var userDAOMock = new Mock<IUserDAO>();
       userDAOMock.Setup(m => m.BuscarPorUsername(It.IsAny<string>())).Returns(Task.FromResult(user));

       var _userAuthenticateService = new UserAuthenticateService(userDAOMock.Object);

       UserSensitive userResponse = await _userAuthenticateService.AuthenticateAsync(userModel);

       userResponse.Should().NotBeNull();
       userResponse.Username.Should().Be(user.Username);
    }
