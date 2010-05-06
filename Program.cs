using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Document;
using Raven.Client;
using Raven.Database.Indexing;
using Raven.Database.Data;
using Newtonsoft.Json.Linq;

namespace Raven.Sample.SimpleClient
{
    class Program
    {
        static void Main(string[] args)
        {
			using (var documentStore = new DocumentStore { Url = "http://localhost:8080" })            
            {
            	documentStore.Initialise();                                                                

			    using (var session = documentStore.OpenSession())
                {
                    //Only add the index if there are not posts in the database, i.e. the 1st time this is run!!
                    if (session.Query<Post>().Count() == 0)
                    {
                        Console.WriteLine("First time usage, creating indexes");
                        documentStore.DatabaseCommands.PutIndex("PostsByTime",
                                                           new IndexDefinition
                                                           {
                                                               Map = @"from post in docs.Posts
                                                                    select new { post.PostedAt }"
                                                           });

                        //documentStore.DatabaseCommands.DeleteIndex("TagCloud");
                        documentStore.DatabaseCommands.PutIndex("TagCloud",
                                                               new IndexDefinition
                                                               {
                                                                   Map = @"from post in docs.Posts                                                                    
                                                                    from Tag in post.Tags
                                                                    select new { Tag, Count = 1 }",

                                                                   Reduce = @"from result in results
                                                                    group result by result.Tag into g
                                                                    select new { Tag = g.Key, Count = g.Sum(x => (long)x.Count) }"
                                                               });
                    }

                    session.Store(new Post { Title = "Title 1", PostedAt = RandomDateTime(), Tags = CreateTags("C#", ".NET") });
                    session.Store(new Post { Title = "Title 1", PostedAt = RandomDateTime(), Tags = CreateTags("C#", ".NET") });
                    session.Store(new Post { Title = "Title 1", PostedAt = RandomDateTime(), Tags = CreateTags("VB", "Java") });
                    session.Store(new Post { Title = "Title 1", PostedAt = RandomDateTime(), Tags = CreateTags("TDD", "Development") });
				    session.SaveChanges();                    

                    var orderedPosts = session
                        .Query<Post>("PostsByTime")
                        .OrderBy("PostedAt")
                        .WaitForNonStaleResults()
                        .ToArray();

                    Console.WriteLine("There are " + orderedPosts.Count() + " documents in the database :");
                    foreach (var post in orderedPosts)
                        Console.WriteLine(post);
                    Console.WriteLine("");
                    
                    var tagCloud = session
                        .Query<TagCloud>("TagCloud")                        
                        .WaitForNonStaleResults()
                        .ToArray();

                    Console.WriteLine("\n\nTagCloud has " + tagCloud.Count() + " items :");
                    foreach (var tagCount in tagCloud)
                        Console.WriteLine("  " + tagCount);                   
                }
            }
        }

        private static List<string> CreateTags(params string [] tags)
        {
            return new List<string>(tags);
        }

        private static DateTime RandomDateTime()
        {
            Random rand = new Random();
            return new DateTime(rand.Next(1900, 2100),  //year
                                rand.Next(1, 12),       //month
                                rand.Next(1, 28),       //day
                                rand.Next(0, 23),       //hour
                                rand.Next(0, 59),       //minute
                                rand.Next(0, 59));      //second
        }
    }    
}
