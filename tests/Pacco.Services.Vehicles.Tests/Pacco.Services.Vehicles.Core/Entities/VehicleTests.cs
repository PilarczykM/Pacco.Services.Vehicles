using Pacco.Services.Vehicles.Core.Entities;
using Pacco.Services.Vehicles.Core.Exceptions;

namespace Pacco.Services.Vehicles.Tests.Pacco.Services.Vehicles.Core.Entities;
public class VehicleTests
{
	const string brand = "Brand";
	const string model = "Model";
	const string description = "Description";
	const double payloadCapacity = 1;
	const double loadingCapacity = 1;
	const decimal pricePerService = 1;

	private static Vehicle CreateVehicle()
		=> new(Guid.NewGuid(), brand, model, description, payloadCapacity, loadingCapacity, pricePerService);

	[Fact]
	public void Create_Vehicle_Throws_InvalidVehicleDescriptionException_When_Description_Not_provided()
	{
		// ARRANGE

		// ACT
		var exception = Record.Exception(() => new Vehicle(Guid.NewGuid(), brand, model, "", payloadCapacity, loadingCapacity, pricePerService));

		// ASSERT
		exception.ShouldNotBeNull();
		exception.ShouldBeOfType<InvalidVehicleDescriptionException>();
		exception.Message.ShouldContain("Vehicle description is invalid");
	}

	[Fact]
	public void Create_Vehicle_Throws_InvalidVehicleCapacity_When_PayloadCapacity_Not_provided()
	{
		// ARRANGE

		// ACT
		var exception = Record.Exception(() => new Vehicle(Guid.NewGuid(), brand, model, description, 0, loadingCapacity, pricePerService));

		// ASSERT
		exception.ShouldNotBeNull();
		exception.ShouldBeOfType<InvalidVehicleCapacity>();
		exception.Message.ShouldContain("Vehicle capacity is invalid");
	}

	[Fact]
	public void Create_Vehicle_Throws_InvalidVehicleCapacity_When_LoadingCapacity_Not_provided()
	{
		// ARRANGE

		// ACT
		var exception = Record.Exception(() => new Vehicle(Guid.NewGuid(), brand, model, description, payloadCapacity, 0, pricePerService));

		// ASSERT
		exception.ShouldNotBeNull();
		exception.ShouldBeOfType<InvalidVehicleCapacity>();
		exception.Message.ShouldContain("Vehicle capacity is invalid");
	}

	[Theory]
	[InlineData(-1)]
	[InlineData(0)]
	public void Create_Vehicle_Throws_InvalidVehiclePricePerServiceException_When_PricePerService_Is_Less_Or_Equal_Zero(int value)
	{
		// ARRANGE

		// ACT
		var exception = Record.Exception(() => new Vehicle(Guid.NewGuid(), brand, model, description, payloadCapacity, loadingCapacity, value));

		// ARRANGE
		exception.ShouldNotBeNull();
		exception.ShouldBeOfType<InvalidVehiclePricePerServiceException>();
		exception.Message.ShouldContain("Vehicle price per service is invalid");
	}

	[Fact]
	public void Create_Vehicle()
	{
		// ARRANGE
		var id = Guid.NewGuid();

		// ACT
		Vehicle vehicle = new(id, brand, model, description, payloadCapacity, loadingCapacity, pricePerService);

		vehicle.ShouldNotBeNull();
		vehicle.Brand.ShouldBe(brand);
		vehicle.Model.ShouldBe(model);
		vehicle.Description.ShouldBe(description);
		vehicle.PayloadCapacity.ShouldBe(payloadCapacity);
		vehicle.LoadingCapacity.ShouldBe(loadingCapacity);
		vehicle.PricePerService.ShouldBe(pricePerService);
		vehicle.Variants.ShouldBe(Variants.Standard);
	}

	[Fact]
	public void ChangeDescription_Change_Description()
	{
		// ARRANGE
		var newDescription = "New Description";
		var vehicle = CreateVehicle();

		// ACT
		vehicle.ChangeDescription(newDescription);

		// ASSERTS
		vehicle.Description.ShouldBe(newDescription);
	}

	[Theory]
	[InlineData("")]
	[InlineData(null)]
	public void ChangeDescription_Throws_InvalidVehicleDescriptionException(string value)
	{
		// ARRANGE
		var vehicle = CreateVehicle();

		// ACT
		var exception = Record.Exception(() => vehicle.ChangeDescription(value));

		// ASSERTS
		vehicle.Description.ShouldBe(description);
		exception.ShouldNotBeNull();
		exception.ShouldBeOfType<InvalidVehicleDescriptionException>();
	}

	[Fact]
	public void ChangePricePerService_Change_Price()
	{
		// ARRANGe
		var vehicle = CreateVehicle();
		var newPricePerService = 2;

		// ACT
		vehicle.ChangePricePerService(newPricePerService);

		// ASSERTS
		vehicle.PricePerService.ShouldBe(newPricePerService);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public void ChangePricePerService_Throws_InvalidVehiclePricePerServiceException(decimal value)
	{
		// ARRANGE
		var vehicle = CreateVehicle();

		// ACT
		var exception = Record.Exception(() => vehicle.ChangePricePerService(value));

		// ASSERTS
		vehicle.PricePerService.ShouldBe(pricePerService);
		exception.ShouldNotBeNull();
		exception.ShouldBeOfType<InvalidVehiclePricePerServiceException>();
	}

	[Theory]
	[InlineData(Variants.Chemistry)]
	[InlineData(Variants.Weapon)]
	[InlineData(Variants.Animal)]
	[InlineData(Variants.Organ)]
	public void AddVariants_Adds_Variants(Variants value)
	{
		// ARRANGE
		var vehicle = CreateVehicle();

		// ACT
		vehicle.AddVariants(value);

		// ASSERTS
		vehicle.Variants.ShouldBe(Variants.Standard | value);
	}

	[Fact]
	public void AddVariants_Adds_Variants_When_Variant_Is_Array()
	{
		// ARRANGE
		var vehicle = CreateVehicle();
		var variants = new Variants[] { Variants.Chemistry, Variants.Weapon };

		// ACT
		vehicle.AddVariants(variants);

		// ASSERTS
		vehicle.Variants.ShouldBe(Variants.Standard | Variants.Chemistry | Variants.Weapon);
	}

	[Theory]
	[InlineData(Variants.Chemistry)]
	[InlineData(Variants.Weapon)]
	[InlineData(Variants.Animal)]
	[InlineData(Variants.Organ)]
	public void AddVariants_Adds_One_Variant_When_Executed_Twice(Variants value)
	{
		// ARRANGE
		var vehicle = CreateVehicle();

		// ACT
		vehicle.AddVariants(value);
		vehicle.AddVariants(value);

		// ASSERTS
		vehicle.Variants.ShouldBe(Variants.Standard | value);
	}

	[Theory]
	[InlineData(Variants.Chemistry)]
	[InlineData(Variants.Weapon)]
	[InlineData(Variants.Animal)]
	[InlineData(Variants.Organ)]
	public void AddVariants_Adds_One_Variant_When_Doubled(Variants value)
	{
		// ARRANGE
		var vehicle = CreateVehicle();

		// ACT
		vehicle.AddVariants(new Variants[] { value, value });

		// ASSERTS
		vehicle.Variants.ShouldBe(Variants.Standard | value);
	}

	[Theory]
	[InlineData(Variants.Chemistry)]
	[InlineData(Variants.Weapon)]
	[InlineData(Variants.Animal)]
	[InlineData(Variants.Organ)]
	public void RemoveVariants_Removes_Variants(Variants value)
	{
		// ARRANGE
		var vehicle = CreateVehicle();

		// ACT
		vehicle.RemoveVariants(value);

		// ASSERTS
		vehicle.Variants.ShouldBe(Variants.Standard);
	}

	[Theory]
	[InlineData(new Variants[] { Variants.Chemistry, Variants.Animal })]
	[InlineData(new Variants[] { Variants.Chemistry, Variants.Weapon })]
	[InlineData(new Variants[] { Variants.Chemistry, Variants.Organ })]
	[InlineData(new Variants[] { Variants.Organ, Variants.Weapon })]
	public void RemoveVariants_Removes_Variants_When_Variant_Is_Array(Variants[] value)
	{
		// ARRANGE
		Vehicle vehicle = new(Guid.NewGuid(), brand, model, description, payloadCapacity, loadingCapacity, pricePerService, value);

		// ACT
		vehicle.RemoveVariants(value);

		// ASSERTS
		vehicle.Variants.ShouldBe(Variants.Standard);
	}
}
