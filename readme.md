NServiceBus.Tracking
====================

An unobtrusive library for keeping track of the flow and progress of messages through an NServiceBus architecture.

###Why do this?

Consider the following scenario:

  * You have a suite of automated, end-to-end tests that run against your system.
  * You have a great REST API that you've put a lot of effort into designing.
  * You *also* have a fire-and-forget, pub/sub-style messaging system under the hood that you use to scale up and provide fast response times.
  * Half your tests are either failing or taking forever to run because the response times from bus endpoints are completely unpredictable and you need explicit (and generous) delays in order to compensate.

The [Request-Acknowledge-Poll](http://gorodinski.com/blog/2012/07/13/request-acknowledge-poll-with-nservicebus-and-aspnet-webapi/) pattern is the canonical solution for fire-and-forget operations in a REST API. You send a `201 Accepted` and provide a status URI in the `Location` header.

And yet, this is *extremely difficult* to do in a large API without incurring the overhead of dozens if not hundreds of additional models, messages, tables, etc.

What would be really useful for this scenario is something that resembles a "batch scheduling" system. In other words, no matter what kind of batch it is, it becomes part of the scheduler, gets its own ID, and can be monitored using a central tool.

This isn't a scheduler. But it does try to provide a universal abstraction for "message chains" so that clients can easily poll for completion as long as they know which message type is supposed to be last in the chain.

As a bonus, it can also be useful as a monitoring tool, or for UI applications that need to poll for completion because they aren't integrated with the messaging system.

##Basic Usage

NServiceBus.Tracking integrates with the Fluent Configuration API of NServiceBus:

```c#
    Configure.With()
        .DefaultBuilder()
        .MsmqTransport()
            .IsTransactional(true)
        .UnicastBus()
            .ImpersonateSender(false)
        .OperationTracking()       // <-- Enables the tracking
        .MongoOperationProvider()  // <-- Store operation info in MongoDB
        .CreateBus()
	    .Start();
```

That's about all there is to it. MongoDB support is included because I work with it on a frequent basis. If you want to use something else, like SQL Server or Oracle, just implement `IOperation` and `IOperationProvider` and configure it in the container.

The source contains a demo app which also illustrates how to set up a custom operation provider (in this case a fake one). Below is an example of how you might use it in a Web API:

```c#
public class PeopleController : ApiController
{
    // Snip private fields and constructors

    public HttpResponseMessage Post(Person person)
    {
        bus.Send(
            new ChangeEmailCommand { Id = person.Id, Email = person.Email },
            new ChangeAddressCommand { Id = person.Id, Address = person.Address }
        );
        var operation = Operation.Current;
        operation.CompleteAfter<ChangeEmailCommand, ChangeAddressCommand>();
        var response = new HttpResponseMessage(HttpStatusCode.Accepted);
        response.Headers.Location = "http://www.example.com/api/status/" + operation.Id;
        return response;
    }
}

public class StatusController : ApiController
{
    // Snip private fields and constructors

    public OperationStatus Get(string id)
    {
        var operation = operationProvider.Find(id);
        if (operation == null)
            throw new HttpResponseException(HttpStatusCode.NotFound);
        return new Status
        {
            Id = operation.Id,
            WhenStarted = operation.WhenStarted,
            IsCompleted = operation.IsCompleted()
        };
    }
}
```

Note how you only need one controller and method to retrieve the status of **any** operation, no matter where it originated! This way, only the *originating* API needs to understand messaging details; the polling API is completely generic.

##(In)frequently Asked Questions:

1. **Doesn't this violate best practices for NServiceBus and messaging in general?**  
   Yes. It's supposed to. It's not intended to be used as a key design element but rather to deal with some very specific situations where the alternatives (callbacks or fire-and-forget) simply aren't practical.

2. **Shouldn't auditing be done asynchronously, i.e. by a separate message handler?**  
   In theory, yes, if you completely trust the messaging system and don't mind the possibility of delayed feedback. I have enough battle scars to not trust MSMQ for anything at all. Also, using a message handler for this would require stuff to be deployed, which is no longer unobtrusive. Besides, this is meant to *supplement* normal messaging, not *replace* it; if you've designed your application correctly, i.e. with the expectation that message processing may fail or be delayed, then it won't matter if the auditing fails.
   
3. **Is this library suitable for production use?**    
   I have no idea. *Caveat emptor*. It's worked for me so far, but that's all I can say. I did note that any exceptions thrown during the `IManageUnitsOfWork.End` method are simply eaten, so even in the worst case, this shouldn't actively do any harm. :)