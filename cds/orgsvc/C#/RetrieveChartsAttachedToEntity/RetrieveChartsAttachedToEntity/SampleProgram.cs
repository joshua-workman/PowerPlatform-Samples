using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerApps.Samples
{
    public partial class SampleProgram
    {
        [STAThread] // Required to support the interactive login experience
        static void Main(string[] args)
        {
            CrmServiceClient service = null;
            try
            {
                service = SampleHelpers.Connect("Connect");
                if (service.IsReady)
                {
                    // Create any entity records that the demonstration code requires
                    SetUpSample(service);

                    #region Demonstrate
                    // TODO Add demonstration code here
                    #endregion Demonstrate
                    // Create a query for retrieving all organization-owned visualizations 
                    // that are attached to the account entity.
                    QueryExpression mySavedQuery = new QueryExpression
                    {
                        EntityName = SavedQueryVisualization.EntityLogicalName,
                        ColumnSet = new ColumnSet("name"),
                        Criteria = new FilterExpression
                        {
                            Conditions =
                         {
                            new ConditionExpression
                               {
                                  AttributeName = "primaryentitytypecode",
                                  Operator = ConditionOperator.Equal,
                                  Values =  {Account.EntityLogicalName}
                               }
                         }
                        }
                    };

                    // Retrieve a collection of organization-owned visualizations that are attached to the account entity.
                    DataCollection<Entity> results = service.RetrieveMultiple(mySavedQuery).Entities;

                    // Display the names of the retrieved organization-owned visualizations.
                    Console.WriteLine("Retrieved the following visualizations for the Account entity:");
                    foreach (Entity entity in results)
                    {
                        SavedQueryVisualization orgVisualization = (SavedQueryVisualization)entity;
                        Console.WriteLine("{0}", orgVisualization.Name);
                    }
                    
                }
                else
                {
                    const string UNABLE_TO_LOGIN_ERROR = "Unable to Login to Common Data Service";
                    if (service.LastCrmError.Equals(UNABLE_TO_LOGIN_ERROR))
                    {
                        Console.WriteLine("Check the connection string values in cds/App.config.");
                        throw new Exception(service.LastCrmError);
                    }
                    else
                    {
                        throw service.LastCrmException;
                    }
                }
            }
            catch (Exception ex)
            {
                SampleHelpers.HandleException(ex);
            }

            finally
            {
                if (service != null)
                    service.Dispose();

                Console.WriteLine("Press <Enter> to exit.");
                Console.ReadLine();
            }

        }
    }
}
