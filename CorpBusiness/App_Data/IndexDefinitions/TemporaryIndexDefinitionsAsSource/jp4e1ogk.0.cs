using Raven.Abstractions;
using Raven.Database.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Raven.Database.Linq.PrivateExtensions;
using Lucene.Net.Documents;
using Raven.Database.Indexing;
public class Index_Temp_2fVendors_2fByEmail : AbstractViewGenerator
{
	public Index_Temp_2fVendors_2fByEmail()
	{
		this.ViewText = @"from doc in docs.Vendors
select new { Email = doc.Email }
";
		this.ForEntityNames.Add("Vendors");
		this.AddMapDefinition(docs => from doc in docs
			where doc["@metadata"]["Raven-Entity-Name"] == "Vendors"
			select new { Email = doc.Email, __document_id = doc.__document_id });
		this.AddField("Email");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("Email");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("Email");
		this.AddQueryParameterForReduce("__document_id");
	}
}
