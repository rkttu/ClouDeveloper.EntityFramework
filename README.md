# ClouDeveloper.EntityFramework

This project contains Entity Framework generic repository implementations. Original code comes from [here](https://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application) with some additional improvements.

You can download this NuGet package with executing command below in Visual Studio Package Console Manager.

```
Install-Package ClouDeveloper.EntityFramework
```

## How to use this package

* Define entity framework context and models.
* Decide wether to use async version or not, and then create a unit of work class.
* Define generic repository private fields with lazy wrapper.
* Define a constructor and initialize lazy factory with CreateEntityFactory(Async) method.
* Define generic repository properties which call lazy wrapper's Value property.

### Non-Async version of UoW class sample (.NET v4+)

```
using System;
using System.Collections.Generic;
using System.Data.Entity;
using ClouDeveloper.EntityFramework;

namespace TestingDemo {
    // Sample code comes from https://msdn.microsoft.com/en-us/library/dn314429(v=vs.113).aspx
    public class BloggingUnitOfWork : UnitOfWork {
        public BloggingUnitOfWork(BloggingContext context) : base(context) {
            this._blogRepositoryFactory = this.CreateEntityFactory<Blog>();
            this._postRepositoryFactory = this.CreateEntityFactory<Post>();
        }

        private Lazy<GenericRepository<Blog>> _blogRepositoryFactory;
        private Lazy<GenericRepository<Post>> _postRepositoryFactory;

        public GenericRepository<Blog> BlogRepository {
            get { return this._blogRepositoryFactory.Value; }
        }

        public GenericRepository<Post> PostRepository {
            get { return this._postRepositoryFactory.Value; }
        }
    }

    public class BloggingContext : DbContext {
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
    }

    public class Blog {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual List<Post> Posts { get; set; }
    }

    public class Post {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
```

### Async version of UoW class sample (.NET v4.5+)

```
using System;
using System.Collections.Generic;
using System.Data.Entity;
using ClouDeveloper.EntityFramework;

namespace TestingDemo {
    // Sample code comes from https://msdn.microsoft.com/en-us/library/dn314429(v=vs.113).aspx
    public class BloggingUnitOfWorkAsync : UnitOfWorkAsync {
        public BloggingUnitOfWorkAsync(BloggingContext context) : base(context) {
            this._blogRepositoryFactory = this.CreateEntityFactoryAsync<Blog>();
            this._postRepositoryFactory = this.CreateEntityFactoryAsync<Post>();
        }

        private Lazy<GenericRepositoryAsync<Blog>> _blogRepositoryFactory;
        private Lazy<GenericRepositoryAsync<Post>> _postRepositoryFactory;

        public GenericRepository<Blog> BlogRepository {
            get { return this._blogRepositoryFactory.Value; }
        }

        public GenericRepository<Post> PostRepository {
            get { return this._postRepositoryFactory.Value; }
        }
    }

    public class BloggingContext : DbContext {
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
    }

    public class Blog {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual List<Post> Posts { get; set; }
    }

    public class Post {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
```

## Contribution

If you interest contributing this project, please send a pull request.

[NuGet Package Web Site](https://www.nuget.org/packages/ClouDeveloper.EntityFramework/)
