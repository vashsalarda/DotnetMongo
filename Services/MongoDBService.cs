using Customer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Customer.Services;

public class MongoDBService
{

	private readonly IMongoCollection<CustomerModel> _customerCollection;

	public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
	{
		MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
		IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
		_customerCollection = database.GetCollection<CustomerModel>(mongoDBSettings.Value.CollectionName);
	}

	public async Task<List<CustomerModel>> GetAsync()
	{
		return await _customerCollection.Find(new BsonDocument()).ToListAsync();
	}

	public async Task CreateAsync(CustomerModel customer)
	{
		await _customerCollection.InsertOneAsync(customer);
		return;
	}

	public async Task<CustomerModel> GetByIdAsync(string id)
	{
		return await _customerCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
	}

	public async Task UpdateAsync(string id, CustomerModel customer)
	{
		FilterDefinition<CustomerModel> filter = Builders<CustomerModel>.Filter.Eq("Id", id);
		var updateList = new List<UpdateDefinition<CustomerModel>>();

		if (customer.FirstName != null)
			updateList.Add(Builders<CustomerModel>.Update.Set(c => c.FirstName, customer.FirstName));

		if (customer.LastName != null)
			updateList.Add(Builders<CustomerModel>.Update.Set(c => c.LastName, customer.LastName));

		if (customer.Country != null)
			updateList.Add(Builders<CustomerModel>.Update.Set(c => c.Country, customer.Country));

		if (customer.State != null)
			updateList.Add(Builders<CustomerModel>.Update.Set(c => c.State, customer.State));

		if (customer.City != null)
			updateList.Add(Builders<CustomerModel>.Update.Set(c => c.City, customer.City));

		UpdateDefinition<CustomerModel> update = Builders<CustomerModel>.Update.Combine(updateList);

		await _customerCollection.UpdateOneAsync(filter, update);
		return;
	}

	public async Task DeleteAsync(string id)
	{
		FilterDefinition<CustomerModel> filter = Builders<CustomerModel>.Filter.Eq("Id", id);
		await _customerCollection.DeleteOneAsync(filter);
		return;
	}

}