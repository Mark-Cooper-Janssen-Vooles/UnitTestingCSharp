# Unit Testing for C# Developers


Contents:
- Getting Started
- Fundametals of Unit Testing
- Core Unit Testing Techniques
- Exercises
- Breaking External Dependencies
- Exercises
- General Methodology for writing Unit Tests


---


## Getting Started
- Our source code consists of application code, and test code. They test each other!
- Automated tests are repeatable, you can check all execution paths super quickly.
- Test your code frequently, in less time
- Catch bugs before deploying
- Deploy with confidence
- Refactor with confidence
- Focus more on the quality


---


### Types of Tests
- Unit tests
  - Tests a unit of an application without its external dependencies (files, db's, message queues etc)
  - Cheap to write
  - Execute fast
  - The more the merrier
  - Don't give as much confidence
- Integration tests
  - Test the application with its external dependencies / recources, like a db. 
  - take longer to execute
  - give more confidence
- End-to-end tests
  - Drives an application through its UI, i.e. Selenium
  - Gives the greatest confidence of health of application
  - Very slow
  - Very brittle


---

### Test Pyramid 
- Argues that mosts tests should be unit tests (easy to write, execute quickly)
- Next most have some integration tests too (more confidence, but slowler)
- Very few E2E for the key functions (happy path) (super confidence, but super slow)
- Takeaways:
  - Favour unit tests over e2e tests 
  - Cover unit tests gaps with integration tests
  - E2E only for key functions and happy paths. Use others for edge-cases


---


### Testing Frameworks
- Most popular for C#:
  - NUnit (one of the earliest ones)
  - MSTest
  - xUnit (gaining popularity)
- They all give a test library and a test runner
- Mosh suggests focus on fundamentals, not the tooling
  - Writing quality tests that give you value


---


### Writing First Test
- Right click project, add, "Unit Test Project". 
- Convention is to name t "<project name>.UnitTests", i.e. ``TestNinja.UnitTests`` in this case
  - You want to separate the unit tests from the integration tests
- When testing a particular class, the convention is to name the test class "<class name>Tests", i.e. ``ReservationTests``
  - all test methods should be "public void"
- When writing tests, name them "<method name>_<scenario>_<expected behavior>", i.e:
  - ``CanBeCancelledBy_Admin_ReturnsTrue``
  - ``CanBeCancelledBy_User_ReturnsTrue``
  - ``CanBeCancelledBy_OtherUser_ReturnsFalse``
- In the body of the test, structure it with "triple A": 
````c#
[TestMethod]
public void CanBeCancelledBy_Scenario_ExpectedBehavior()
{
    //Arrange - initialise objects

    //Act - the action you want to test

    //Assert - the result you want to test
}
````
````c#
[TestClass]
public class ReservationTests
{
    [TestMethod]
    public void CanBeCancelledBy_Scenario_ExpectedBehavior()
    {
        //Arrange
        var reservation = new Reservation();
        var user = new User() { IsAdmin = true };

        //Act
        var result = reservation.CanBeCancelledBy(user);

        //Assert
        //using the ms test framework:
        Assert.IsTrue(result);
    }
}
````


---


### Using NUnit in Visual studio
- Not part of visual studio, if you want to use it you need to install nuget packages
  - open "package manager console" and type ``install-package NUnit -Version 3.8.1``
  - ``install-package NUnit3TestAdapter -Version 3.8.0``
- Instead of [TestClass] (ms test framework), we use [TestFixrue] (NUnit)
- We have a more readable way of writing assertions
````c#
Assert.IsTrue(result); //still valid with nunit
Assert.That(result, Is.True);
Assert.That(result === true);
````


---


## Fundamentals of Unit Testing
- Charactristics of a good unit test
- What to test, what not to test
- Naming and organising tests
- Basic Techniques
- Writing reliable tests


---


### Characteristics of Good Unit Tests
- First-class citizens: as important as production code 
- Clean, readable, maintainable
- No logic (no if, else, foreach etc) - simply call a method, make an assertion
- Each test is isolated - each test method should not call each other or assume other state created by another test
- Not too specific or too general


"Each test needs to look like its the only test in the world", i.e.:
````c#
[TextFixture]
public class MathTests
{
  private Math _math = new Math(); //don't do this! It is possible that some tests will alter this object, and break the next test.

  [Test]
  public void Test1() {...}

  [Test]
  public void Test2() {...}
}

//instead do this:
[TextFixture]
public class MathTests
{
  [Test]
  public void Test1() 
  {
    var math = new Math();
    //logic...
  }

  [Test]
  public void Test2() 
  {
    var math = new Math();
    //logic...
  }
}
````


---


### What to test and what not to test
- A lot of times when you don't know what to test, its because you're dealing with poorly written code. Methods with more than 10 lines of code etc. 
- Unit testing and clean coding go hand in hand
- Assuming code is clean, test the outcome of a function.
  - We have two types of functions: Query's and commands.
  - Querys:
    - return some value
    - verify the function is returning the right value
    - test all the execution paths
  - Command function:
    - returns an action
    - i.e. writing to the database, or making a change to the system
    - may return a value
    - test the outcome of the method, i.e. has the test made the right call to the external dependency? (i.e. the db)
- What not to test:
  - language features
  - 3rd-party code, i.e. don't test entity framework method. assume they are properly tested. only test your code.


---


### Naming and Organising tests


Organising:
- Each project should have a unit testing project associated with it
  - i.e. TestNinja (proj) has TestNinja.UnitTests (proj)
- In the unit test project, there should be a test class for each class in the code
  - i.e. Reservation should have ReservationTests
- For each method in the class, there should be one or more unit test methods
  - how many do you need? it depends on the method in the class. 
  - the number of tests should be >= the number of execution paths


Naming:
- The name of your tests should clearly specify the business rule you're testing
- Convention for naming tests:
  - [MethodName]_[Scenario]_[ExpectedBehaviour]
- Sometimes it may be better to dedicate a seperate test class for one method in a class, if it requires lots of tests
  - i.e. a method in the Reservation class called "CanBeCancelledBy()" has lots of execution paths, we can make another test called:
  "Reservation_CanBeCancelledByTests"


---


### NUnit: Setup and Tear Down - a neater way to write tests
- You can use the SetUp attribute to create a method and decorate it with the SetUp attribute
  - NUnit test runner will call that test method before running each test, i.e. a good oppourtunity for common arrange code lines.
- If we create a method decorated with TearDown attribute, NUnit test runner will call that method after each test. Usually only associated with integration tests, i.e. databases. 
- Without: 
````c#
[TestFixture]
public class MathTests
{
    [Test]
    public void Add_WhenCalled_ReturnTheSumOfArguments()
    {
        //arrange
        var math = new Math();
        
        //act
        var result = math.Add(2, 2);
        
        //assert
        Assert.That(result, Is.EqualTo(4));
    }

    [Test]
    public void Max_WhenCalledWithLargestNumberFirst_ReturnsFirstNumber()
    {
        //arrange
        var math = new Math();
        //act
        var result = math.Max(2, 1);
        //assert
        Assert.That(result, Is.EqualTo(2));
    }
    
    [Test]
    public void Max_WhenCalledWithLargestNumberSecond_ReturnsSecondNumber()
    {
        //arrange
        var math = new Math();
        //act
        var result = math.Max(1, 2);
        //assert
        Assert.That(result, Is.EqualTo(2));
    }
    
    [Test]
    public void Max_CalledWithEqualNumbers_ReturnsSameNumber()
    {
        //arrange
        var math = new Math();
        //act
        var result = math.Max(2, 2);
        //assert
        Assert.That(result, Is.EqualTo(2));
    }
}
````
- With: 
````c#
[TestFixture]
public class MathTests
{
    private Math _math;
    [SetUp]
    public void SetUp()
    {
        _math = new Math();
    }
    
    [Test]
    public void Add_WhenCalled_ReturnTheSumOfArguments()
    {
        var result = _math.Add(2, 2);
        Assert.That(result, Is.EqualTo(4));
    }

    [Test]
    public void Max_WhenCalledWithLargestNumberFirst_ReturnsFirstNumber()
    {
        var result = _math.Max(2, 1);
        Assert.That(result, Is.EqualTo(2));
    }
    
    [Test]
    public void Max_WhenCalledWithLargestNumberSecond_ReturnsSecondNumber()
    {
        var result = _math.Max(1, 2);
        Assert.That(result, Is.EqualTo(2));
    }
    
    [Test]
    public void Max_CalledWithEqualNumbers_ReturnsSameNumber()
    {
        var result = _math.Max(2, 2);
        Assert.That(result, Is.EqualTo(2));
    }
}
````


---


### Parameterized Tests for NUnit
- When tests look very same and just test different values, thats a case for parameterizing tests
- Without parameterizing: 
````c#
//there was 3 of these all very similar
[Test]
public void Max_WhenCalledWithLargestNumberSecond_ReturnsSecondNumber()
{
    var result = _math.Max(1, 2);
    Assert.That(result, Is.EqualTo(2));
}
````
- With paramterizing:
````c#
// each "testCase" counts as a new test. You can't do this in ms test (but i think you can in XUnit)
[Test]
[TestCase(2, 1, 2)]
[TestCase(1, 2, 2)]
[TestCase(1, 1, 1)]
public void Max_WhenCalled_ReturnsLargerArgument(int a, int b, int expectedResult)
{
    var result = _math.Max(a, b);
    Assert.That(result, Is.EqualTo(expectedResult));
}
````


---


### Ignoring tests (NUnit)
````c#
[Test]
[Ignore("the reason for ignoring")] //this will ignore the test, better than commenting it out
public void Max_WhenCalledWithLargestNumberSecond_ReturnsSecondNumber()
{
    var result = _math.Max(1, 2);
    Assert.That(result, Is.EqualTo(2));
}
````

---


### Writing Trustworthy Tests
- Use TDD
- Writing code first is more untrustworthy 
  - An untrustworthy test is something like a test with a bug
- How to write trustworthy tests if coding first:
  - Go into the code, and on the line thats supposed to make the test pass, change its value. Create a bug purposefully. Does the test still pass? If no, then it might be untrustworthy. 


---


## Core Unit Testing Techniques


### Testing Strings
````c#
[Test]
public void FormatAsBold_WhenCalled_ShouldReturnStringWrappedInStrongTag()
{
    var htmlFormatter = new HtmlFormatter();
    var result = htmlFormatter.FormatAsBold("Hello World");
    Assert.That(result, Is.EqualTo("<strong>Hello World</strong>")); //this is specific
    //by default, assertions against strings are case-sensitive. To ignore case:
    Assert.That(result, Is.EqualTo("<strong>Hello World</strong>").IgnoreCase);
    
    //more general ways to test, often good for testing strings:
    Assert.That(result, Does.StartWith("<strong>"));
    Assert.That(result, Does.EndWith("</strong>")); 
    Assert.That(result, Does.Contain("Hello World")); 
}
````


---


### Testing Arrays and Collections
- When dealing with arrays, you want them to have more than 1 item
````c#
[Test]
//[TestCase(5, new [] {1, 3, 5})]
public void GetOddNumbers_WhenCalled_ReturnsEnumerableOfOddNumbersToMaxNumber()
{
    var result = _math.GetOddNumbers(5);

    //general
    Assert.That(result, Is.Not.Empty);
    Assert.That(result.Count(), Is.EqualTo(3));
    //more specific
    Assert.That(result, Does.Contain(1));
    Assert.That(result, Does.Contain(3));
    Assert.That(result, Does.Contain(5));
    //hyper specific
    Assert.That(result, Is.EqualTo(new [] {1, 3, 5}));

    //other possible useful assertions:
    Assert.That(result, Is.Ordered);
    Assert.That(result, Is.Unique);
}
````


---


### Testing The Return Type of Methods
````c#
[Test]
public void GetCustomer_WhenCalledWithIdZero_ReturnsNotFound()
{
    var customerController = new CustomerController();

    var result = customerController.GetCustomer(0);

    Assert.That(result, Is.TypeOf<NotFound>()); //exactly a NotFound object
    //Assert.That(result, Is.InstanceOf<NotFound>()); //a NotFound object, or one of its derivitives
}

[Test]
public void GetCustomer_WhenCalled_ReturnsOk()
{
    var customerController = new CustomerController();

    var result = customerController.GetCustomer(1);

    Assert.That(result, Is.TypeOf<Ok>());
}
````


---


### Testing Void Methods
- Previously we talked about query functions and command functions
- Void methods, by definition, are command functions
- They either change the state of an object, the value of one or more properties, or may persist the state (in db, call a message queue, raise an event, etc)
- This example test that a field in an object was set:
````c#
[TestFixture]
public class ErrorLoggerTests
{
    [Test]
    public void Log_WhenCalled_ShouldSetLastErrorProperty()
    {
        var logger = new ErrorLogger();
        
        logger.Log("a"); // Log() should set "LastError" field to whatever string is passed in

        Assert.That(logger.LastError, Is.EqualTo("a"));
    }
}
````


---


### Testing Methods that throw Exceptions
- Make sure to test all the possible paths
````c#
[Test]
[TestCase(null)]
[TestCase("")]
[TestCase(" ")]
public void Log_WhenCalledIncorrectly_ShouldThrowArgumentNullException(string error)
{
    var logger = new ErrorLogger();
    Assert.That(() => logger.Log(error), Throws.ArgumentNullException);
    // for custom exceptions etc you can use something like this:
    //Assert.That(() => logger.Log(error), Throws.Exception.TypeOf<ArgumentNullException>());
}
````


---


### Testing Methods that Raise an Event
````c#
[Test]
public void Log_ValidError_RaiseErrorLoggedEvent()
{
    var logger = new ErrorLogger();

    var id = Guid.Empty;
    logger.ErrorLogged += (sender, args) => { id = args; };
    
    logger.Log("a");

    Assert.That(id, Is.Not.EqualTo(Guid.Empty));
}
````


---


### Testing Private Methods
- How to test private/protected members? You shouldn't!
  - Tests become fragile and break often
  - Think of a blackbox - you're not supposed to know how it works on the inside 
  - An impelemtation detail (aka in the black box) can change
  - If you test private methods, your tests are coupled to the implementation details. When you change that, your tests will break and slow you down
- Public members are the buttons on the outside of the black box => should be tested - the public interface!
- Private/protected members are the implementation detail of the black box => don't test


---


# Breaking External Dependencies
- How do we test a class that depends on an external resource? (aka a DB)
  - Unit tests should not touch external resources, this is an integration test
- You need to decouple the class in some way, i.e. dependency injection mocking
  - a "fake" or a "Test double" with replace the external dependency class

### Loosely-coupled and testable code
- Three steps to follow to achieve testable, loosely-coupled design:
  1. Extract the code that uses an external resource into a seperate class
    - isolating it from the rest of your code
  2. Extract an interface from that class
    - an interface is just the contract, there is no implementation
  3. Modify the test to talk to the interface, as opposed to one of its (or its only) concrete implementation
    - instead of being dependent on a specific implementation (including the external resource), it will be dependent only on the interface / contract 
- In practical terms:
  - Delete the lines where you create an instance of that implementation using the new operator
    - When you use the new operator inside a class, you're making it tightly coupled 
    ````c#
    //tightly coupled, can't unit test, can only integration test
    public void MyMethod()
    {
      var reader = new FileReader();
      reader.Read();
    }

    //loosely coupled, can unit test
    public void MyMethod(IFileReader reader) //can put a mock of IFileReader in here when testing
    {
      reader.Read();
    }
    ````
- This is "DEPENDENCY INJECTION". Instead of newing up dependencies, we inject them from the outside!



---


### Refactoring Towards Loosely-Coupled design example
- Before:
````c#
public class VideoService
{
    public string ReadVideoTitle()
    {
        var str = File.ReadAllText("video.txt");
        var video = JsonConvert.DeserializeObject<Video>(str);
        if (video == null)
            return "Error parsing the video.";
        return video.Title;
    }
}
````

- ReadVideoTitle() is tightly coupled to "File" => Put that in its own flass "FileReader" + give it an interface IFileReader
- Change ReadVideoTitle() to use the new class, with an injection type as IFileReader 
- When making tests, make a new file called "MockFileReader" which inherits from IFileReader but just returns an empty string (rather than using the external dependency, File and the name of a text file .. which would be an integration test)

````c#
public interface IFileReader
{
    string Read(string path);
}

public class FileReader : IFileReader
{
    public string Read(string path)
    {
        return File.ReadAllText(path);
    }
}

public class VideoService
{
    public string ReadVideoTitle(IFileReader fileReader) //this is just one way to use dependency injection
    {
        var str = fileReader.Read("video.txt");
        var video = JsonConvert.DeserializeObject<Video>(str);
        if (video == null)
            return "Error parsing the video.";
        return video.Title;
    }
}

//in the test file "MockFileReader.cs":
public class MockFileReader : IFileReader
{
    public string Read(string path)
    {
        return ""; //can now use this, and test it without any external dependency
    }
}
````


---


### Three Ways to use dependency injection:
- Via method parameters
- Via Properties
- Via Constructor



---


### Dependency Injection via Method Parameters
````c#
public string ReadVideoTitle(IFileReader fileReader)
{
    var str = fileReader.Read("video.txt");
    var video = JsonConvert.DeserializeObject<Video>(str);
    if (video == null)
        return "Error parsing the video.";
    return video.Title;
}

public class Program
{
    public static void Main()
    {
        var service = new VideoService();
        var title = service.ReadVideoTitle(new FileReader());
        // the above is jsut an example. In the real world instead of us manually newing up objects, we use a dependency injection framework
    }
}

//==========
//Testing looks like this: 
[Test]
public void ReadVideoTitle_EmptyFile_ReturnError()
{
    var service =new VideoService();

    var result = service.ReadVideoTitle(new MockFileReader());

    Assert.That(result, Does.Contain("error").IgnoreCase);
}
````


---


### Dependency injection via Properties
- While dependency injection via method parameters works, you may run into issues:
  - you're changing the signature of the method: if you've used it in 10 places in your code, you'll have to modify all 10 manually
  - some dependency injection frameworks cannot inject via method parameters
````c#
public class VideoService
{
    public IFileReader FileReader { get; set; }

    public VideoService()
    {
        FileReader = new FileReader();
    }

    public string ReadVideoTitle()
    {
        var str = FileReader.Read("video.txt");
        var video = JsonConvert.DeserializeObject<Video>(str);
        if (video == null)
            return "Error parsing the video.";
        return video.Title;
    }
}

//the test now looks like:
[Test]
public void ReadVideoTitle_EmptyFile_ReturnError()
{
    var service = new VideoService();
    service.FileReader = new MockFileReader(); //need to set this to the mock to decouple it! a bit easier to miss.

    var result = service.ReadVideoTitle();

    Assert.That(result, Does.Contain("error").IgnoreCase);
}
````


---


### Dependency Injection via Constructor
- some dependency injection frameworks can't inject via properties either, so can inject via constructor
````c#
public class VideoService
{
    private IFileReader _fileReader;

    public VideoService()
    {
        _fileReader = new FileReader(); //used in live code as not to break any tests
    }

    public VideoService(IFileReader fileReader)
    {
        _fileReader = fileReader; //this one used in tests, to pass a mock file reader
    }
}
````
- when you change the constructor, changes are you've broken the code somewhere else too, suggests alternatives:
  - make a constructor without the fileReader argument. Use the one without for live code, and new it up in the constructor + use the one with the constructor for your tests to pass in the mock fileReader
  - combine the constructors:
  ````c#
  public class VideoService
  {
      private IFileReader _fileReader;
      
      //the below is a bit ugly, referred to as "Poor mans dependency injection"
      public VideoService(IFileReader fileReader = null) //gives it a default of null - pass file reader in unit tests
      {
          _fileReader = fileReader ?? new FileReader(); //if not null, new it up
      }
  }
  ````
- the above code allows the existing program to be unchanged, while also allowing tests to work with mock objects.


---


### Dependency Injection Frameworks
- using a proper dependency injection framework, you can simplify the above "poor mans" code to something like this:
````c#
public class VideoService
{
    private IFileReader _fileReader;

    public VideoService(IFileReader fileReader)
    {
        _fileReader = fileReader;
    }
}
````
- dependency injection framework takes care of creating and initialising objects at runtime
- dependency injection frameworks:
  - NInject
  - StructureMap
  - Spring.NET
  - Autofac
  - Unity
- A DI framework has a container, which is a registery for all your interfaces and their implementations
  - when the app starts, the DI framework will take care of creating these objects
  - i.e. when an .NET API receieves a request, the app will create an instance of a controller class
    - controller might have 1 or more dependencies
    - dependency injection framework then kicks in, looks up paramaters, finds the interface and the concrete implementation for the interface, then news it up and passes it to the controller 
- Specific dependency injection frameworks are not part of this course, theres lots of them and their implementation differs based on what kind of app you're creating
  - Mosh recommends either NInject or Autofac



---


### Mocking Frameworks 
- We've created a mock class now by using an interface to create it, but this only works for one code path. We'd have to create all the objects for all the code paths by hand which would take a long time
- Instead better to use a mocking framework - creates them dynamically quickly
- Examples:
  - Moq (preferred!)
  - NSubstitute
  - FakeItEasy
  - Rhino Mocks



---


### Creating Mock Objects using Moq
- Right click the tests project => Manage NuGet Packages => search for "Moq" => click the "+"
- To find the complete list of what Moq can do: https://github.com/Moq/moq4/wiki/Quickstart
- Use mocks only for external dependencies or they will clutter the code
````c#
[TestFixture]
public class VideoServiceTests
{
    [Test]
    public void ReadVideoTitle_EmptyFile_ReturnError()
    {
        var fileReader = new Mock<IFileReader>();
        fileReader.Setup(fr => fr.Read("video.txt")).Returns("");
        
        var service = new VideoService(fileReader.Object); //this gets the actual object itself that implements IFileReader

        var result = service.ReadVideoTitle();
        Assert.That(result, Does.Contain("error").IgnoreCase);
    }
}
````
- Mosh prefers to put Moq's in the setup: 
````c#
[TestFixture]
public class VideoServiceTests
{
    private VideoService _videoService;
    private Mock<IFileReader> _fileReader;

    [SetUp]
    public void SetUp()
    {
        _fileReader = new Mock<IFileReader>();
        _videoService = new VideoService(_fileReader.Object);
    }
    
    [Test]
    public void ReadVideoTitle_EmptyFile_ReturnError()
    {
        _fileReader.Setup(fr => fr.Read("video.txt")).Returns("");

        var result = _videoService.ReadVideoTitle();
        Assert.That(result, Does.Contain("error").IgnoreCase);
    }
}
````


---


### State-based vs Interaction Testing
- So far we've been doing state-based testing, i.e. if the state of an object has changed, if a method has returned something, etc
- Sometimes we need to test if the code interacts with another class properly, aka "Interaction Testing"
- IMPORTANT: use interaction testing only when dealing with external resources
  - because with interaction testing they start to couple with implementation, refactoring and restructuring could break a few tests
- Tests to test the external behaviour and not the implementation: Prefer state-based testing
````c#
//In this example, when testing PlaceOrder(), we'd need to check if the storage object was created and Store() called correctly
public class OrderService
{
    public void PlaceOrder(Order order)
    {
      _storage.Store(order);
      //...
    }
}
````


---


### Interaction Testing Example
````c#
[TestFixture]
public class OrderServiceTests
{
    [Test]
    public void PlaceOrder_WhenCalled_ShouldStoreTheOrder()
    {
        var storage = new Mock<IStorage>();
        var orderService = new OrderService(storage.Object);
        var order = new Order();
        
        orderService.PlaceOrder(order);
        
        storage.Verify(s => s.Store(order)); //.Verify verifies that Store() was called with the same order
    }
}
````


---


### Fake as Little as Possible
- Use mocks as little as possible
  - only when dealing with external resources, unless...
  - if theres complex interactions between between ClassA and ClassB, it might be appropriate to use a mock for classB


---


### Abusing Mocks
- In this example, there is no external resources nor complex interactions. Mock abuse! Test is bulkier.
````c#
[Test]
public void GetPrice_WhenCustomerIsGold_ShouldGive30PercentDiscount()
{
    var product = new Product() { ListPrice = 10.0f };

    var price = product.GetPrice(new Customer() {IsGold = true});

    Assert.That(price, Is.EqualTo(7.0f));
}

[Test]
public void GetPrice_WhenCustomerIsGold_MockExample()
{
    var customer = new Mock<ICustomer>();
    customer.Setup(c => c.IsGold).Returns(true); //an example of mock abuse, for something so simple
    
    var product = new Product() { ListPrice = 10.0f };
    
    var price = product.GetPrice(customer.Object);

    Assert.That(price, Is.EqualTo(7.0f));
}
````


---


### Exercises
- Anything that touches an external resource should be refactored out into its own method 
- Things that touch the DB should use the "repository pattern" (in Mosh's entity framework course), basically just make a file called "VideoRepository" 
- If a dependency is used in a class only for a single method, from a design perspective its better to just pass that dependency to that method directly, otherwise your class constructor can get too bulky (depending on the dependency injection framework you're using - injecting directly to methods may or may not be supported). This is also a sign that the class is doing too much
- Moq mock objects weird calling behaviour: 
````c#
[Test]
public void DownloadInstaller_GivenErrorIsThrown_ReturnsFalse()
{
    _installerHelperRepository.Setup(ihr => ihr.Download("", "")).Throws<WebException>(); //doesn't actually throw, because the method Download() is not called with the same arguments as expected from DownloadInstaller

    var result = _installerHelper.DownloadInstaller("Customer", "Installer");

    Assert.That(result, Is.EqualTo(false));
}

//need to do this instead: 
[Test]
public void DownloadInstaller_GivenErrorIsThrown_ReturnsFalse()
{
    _installerHelperRepository.Setup(ihr => ihr.Download($"http://example.com/Customer/Installer", @"C:\dev")).Throws<WebException>();

    var result = _installerHelper.DownloadInstaller("Customer", "Installer");

    Assert.That(result, Is.EqualTo(false));
}

//or can do this (recommended!)
[Test]
public void DownloadInstaller_GivenErrorIsThrown_ReturnsFalse()
{
    _installerHelperRepository.Setup(ihr => ihr.Download($"http://example.com/Customer/Installer", @"C:\dev")).Throws<WebException>();

    var result = _installerHelper.DownloadInstaller("Customer", "Installer");

    Assert.That(result, Is.EqualTo(false));
}
````


- Handy thing to know about IQueryable<object>:
````c#
[Test]
public void OverlappingBookingsExist_OverlappingBookingArrivalDate_ReturnsBookingReference()
{
    //arrange
    var bookingRepository = new Mock<IBookingRepository>();
    var bookingHelper = new BookingHelper(bookingRepository.Object);
    
    var newBooking = new Booking(){ ArrivalDate = new DateTime(2020, 1, 1)};
    var existingBooking = new Booking()
    {
        ArrivalDate = new DateTime(2020, 1, 1),
        DepartureDate = new DateTime(2020, 2, 1),
        Reference = "1"
    };
    bookingRepository.Setup(b => b.GetActiveBookings(newBooking)).Returns(new List<Booking>() { existingBooking }.AsQueryable()); //AsQueryable turns a list into an IQueryable<Book>
    
    //act
    var result = bookingHelper.OverlappingBookingsExist(newBooking);
    
    //assert
    Assert.That(result, Is.EqualTo("1"));
}
````


- Handy thing to test a method is never called:
````c#
_emailSender.Verify(es => 
        es.EmailFile(It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>()), Times.Never()); //the Times.Never()
````


---


### General Methodology for writing Unit Tests
1. Refactor any external dependencies: things that touch external services (like DB), or any complex class relations. Refactor them to be dependency injected, so you are just testing your class.
2. When writing tests, mock the interfaces.
3. Only test your public methods
4. Consider the test scenarios, treat your public method like a black box. Consider any pitfalls without looking into the code. Is your method a command or a query method?
  - For query method, test you are returning the right thing (i.e. Assert.That(price, Is.EqualTo(7.0f));)
  - For command, test are calling the right methods (i.e. .Verify(es => es.Email()))
5. Write all your tests
6. Refactor tests - pull any duplicated info into other methods (i.e. Setup), private attributes on the test class, helper methods etc. Tests should only be 4 lines max ideally!
7. Wherever you can, use parameterized tests
