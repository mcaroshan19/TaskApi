using APITask.Model;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APITask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly SqlConnection _Con;

        public CustomerController(SqlConnection con)
        {
            _Con = con;
        }

        [HttpPost]
        public IActionResult Create(InvoiceDetails obj)
        {
            if (obj == null)
            {
                return BadRequest("Invoice details cannot be null.");
            }

            string Msg = "";

            try
            {
                using (SqlCommand cmd = new SqlCommand("InsertInvoiceDetails", _Con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@InvoiceNumber", obj.InvoiceNumber);
                    cmd.Parameters.AddWithValue("@InvoiceDate", obj.InvoiceDate);
                    cmd.Parameters.AddWithValue("@Customer", obj.Customer);
                    cmd.Parameters.AddWithValue("@OfferNumber", obj.OfferNumber);
                    cmd.Parameters.AddWithValue("@PartNumber", obj.PartNumber);
                    cmd.Parameters.AddWithValue("@Qty", obj.Qty);
                    cmd.Parameters.AddWithValue("@Rate", obj.Rate);
                    cmd.Parameters.AddWithValue("@Taxable", obj.Taxable);

                    cmd.Parameters.AddWithValue("@LandedCost", obj.LandedCost);
                    cmd.Parameters.AddWithValue("@Profitability", obj.Profitability);
                    cmd.Parameters.AddWithValue("@SalesExecutive", obj.SalesExecutive);

                    _Con.Open();
                    int result = cmd.ExecuteNonQuery();
                    _Con.Close();

                    Msg = result > 0 ? "Data has been inserted successfully." : "Failed to insert data.";
                }

                return Ok(Msg);
            }
            catch (Exception ex)
            {
                _Con.Close();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            List<InvoiceDetails> invoices = new List<InvoiceDetails>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetAllInvoiceDetails", _Con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    _Con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            invoices.Add(new InvoiceDetails
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                InvoiceNumber = reader["InvoiceNumber"].ToString(),
                                InvoiceDate = Convert.ToDateTime(reader["InvoiceDate"]),
                                Customer = reader["Customer"].ToString(),
                                OfferNumber = reader["OfferNumber"].ToString(),
                                PartNumber = reader["PartNumber"].ToString(),
                                Qty = Convert.ToInt32(reader["Qty"]),
                                Rate = Convert.ToDecimal(reader["Rate"]),
                                Taxable = Convert.ToDecimal(reader["Taxable"]),
                                LandedCost = Convert.ToDecimal(reader["LandedCost"]),
                                Profitability = Convert.ToDecimal(reader["Profitability"]),
                                SalesExecutive = reader["SalesExecutive"].ToString()
                            });
                        }
                    }
                    _Con.Close();
                }

                return Ok(invoices);
            }
            catch (Exception ex)
            {
                _Con.Close();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        





        //[HttpPost("externalData")]
        //public IActionResult UpdateInvoice([FromBody] JsonElement externalData)
        //{
        //    try
        //    {

        //        var invoiceDto = new InvoiceDetails
        //        {
        //            Id = externalData.GetProperty("Id").GetInt32(),
        //            InvoiceNumber = externalData.GetProperty("InvoiceNumber").GetString(),
        //            InvoiceDate = externalData.TryGetProperty("InvoiceDate", out var invoiceDate)
        //                          ? DateTime.Parse(invoiceDate.GetString())
        //                          : DateTime.Now,
        //            Customer = externalData.GetProperty("Customer").GetString(),
        //            OfferNumber = externalData.GetProperty("OfferNumber").GetString(),
        //            PartNumber = externalData.GetProperty("PartNumber").GetString(),
        //            Qty = externalData.GetProperty("Qty").GetInt32(),
        //            Rate = externalData.GetProperty("Rate").GetDecimal(),
        //            Taxable = externalData.GetProperty("Taxable").GetDecimal(),
        //            LandedCost = externalData.GetProperty("LandedCost").GetDecimal(),
        //            Profitability = externalData.GetProperty("Profitability").GetDecimal(),
        //            SalesExecutive = externalData.GetProperty("SalesExecutive").GetString()
        //        };

        //        using (SqlCommand cmd = new SqlCommand("InsertInvoiceDetails", _Con))
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@Id", invoiceDto.Id);
        //            cmd.Parameters.AddWithValue("@InvoiceNumber", invoiceDto.InvoiceNumber);
        //            cmd.Parameters.AddWithValue("@InvoiceDate", invoiceDto.InvoiceDate);
        //            cmd.Parameters.AddWithValue("@Customer", invoiceDto.Customer);
        //            cmd.Parameters.AddWithValue("@OfferNumber", invoiceDto.OfferNumber);
        //            cmd.Parameters.AddWithValue("@PartNumber", invoiceDto.PartNumber);
        //            cmd.Parameters.AddWithValue("@Qty", invoiceDto.Qty);
        //            cmd.Parameters.AddWithValue("@Rate", invoiceDto.Rate);
        //            cmd.Parameters.AddWithValue("@Taxable", invoiceDto.Taxable);
        //            cmd.Parameters.AddWithValue("@LandedCost", invoiceDto.LandedCost);
        //            cmd.Parameters.AddWithValue("@Profitability", invoiceDto.Profitability);
        //            cmd.Parameters.AddWithValue("@SalesExecutive", invoiceDto.SalesExecutive);

        //            _Con.Open();
        //            cmd.ExecuteNonQuery();
        //            _Con.Close();
        //        }

        //        return Ok("Invoice updated successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        if (_Con.State == System.Data.ConnectionState.Open)
        //        {
        //            _Con.Close();
        //        }
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}


        //[HttpPost("externalData")]
        //public IActionResult UpdateInvoice([FromBody] JsonElement externalData)
        //{
        //    try
        //    {
        //        // Ensure all required properties are available in the JSON input
        //        if (!externalData.TryGetProperty("InvoiceNumber", out var invoiceNumber) ||
        //            !externalData.TryGetProperty("Customer", out var customer) ||
        //            !externalData.TryGetProperty("OfferNumber", out var offerNumber) ||
        //            !externalData.TryGetProperty("PartNumber", out var partNumber) ||
        //            !externalData.TryGetProperty("Qty", out var qty) ||
        //            !externalData.TryGetProperty("Rate", out var rate) ||
        //            !externalData.TryGetProperty("Taxable", out var taxable) ||
        //            !externalData.TryGetProperty("LandedCost", out var landedCost) ||
        //            !externalData.TryGetProperty("Profitability", out var profitability) ||
        //            !externalData.TryGetProperty("SalesExecutive", out var salesExecutive))
        //        {
        //            return BadRequest("Required property is missing in the request.");
        //        }

        //        // Convert JsonElement properties to appropriate types
        //        var invoiceDto = new InvoiceDetails
        //        {
        //            // We can handle Id as auto-generated (if needed, remove from API or set to 0 for auto-generation)
        //            InvoiceNumber = invoiceNumber.GetString(),
        //            InvoiceDate = externalData.TryGetProperty("InvoiceDate", out var invoiceDate)
        //                ? DateTime.Parse(invoiceDate.GetString())
        //                : DateTime.Now,
        //            Customer = customer.GetString(),
        //            OfferNumber = offerNumber.GetString(),
        //            PartNumber = partNumber.GetString(),
        //            Qty = qty.GetInt32(),
        //            Rate = rate.GetDecimal(),
        //            Taxable = taxable.GetDecimal(),
        //            LandedCost = landedCost.GetDecimal(),
        //            Profitability = profitability.GetDecimal(),
        //            SalesExecutive = salesExecutive.GetString()
        //        };

        //        // Ensure the connection string is valid
        //        using (SqlConnection conn = new SqlConnection(_Con.ConnectionString))
        //        {
        //            using (var cmd = new SqlCommand("InsertInvoiceDetailss", conn))
        //            {
        //                cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //                // Add parameters to the stored procedure
        //                cmd.Parameters.AddWithValue("@InvoiceNumber", invoiceDto.InvoiceNumber);
        //                cmd.Parameters.AddWithValue("@InvoiceDate", invoiceDto.InvoiceDate);
        //                cmd.Parameters.AddWithValue("@Customer", invoiceDto.Customer);
        //                cmd.Parameters.AddWithValue("@OfferNumber", invoiceDto.OfferNumber);
        //                cmd.Parameters.AddWithValue("@PartNumber", invoiceDto.PartNumber);
        //                cmd.Parameters.AddWithValue("@Qty", invoiceDto.Qty);
        //                cmd.Parameters.AddWithValue("@Rate", invoiceDto.Rate);
        //                cmd.Parameters.AddWithValue("@Taxable", invoiceDto.Taxable);
        //                cmd.Parameters.AddWithValue("@LandedCost", invoiceDto.LandedCost);
        //                cmd.Parameters.AddWithValue("@Profitability", invoiceDto.Profitability);
        //                cmd.Parameters.AddWithValue("@SalesExecutive", invoiceDto.SalesExecutive);

        //                conn.Open();
        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        return Ok("Invoice inserted successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}




        [HttpPost("externalData")]
        public IActionResult UpdateInvoice([FromBody] JsonElement externalData)
        {
            try
            {
                // Create a new InvoiceDetails object
                var invoiceDto = new InvoiceDetails();

                // A dictionary to map alternate parameter names to model properties
                var parameterMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "InvoiceNumber", "InvoiceNumber" },
            { "Invoiceno", "InvoiceNumber" },
            { "InvoiceNum", "InvoiceNumber" },
            { "InvoiceDate", "InvoiceDate" },
            { "Customer", "Customer" },
            { "OfferNumber", "OfferNumber" },
            { "PartNumber", "PartNumber" },
            { "Qty", "Qty" },
            { "Rate", "Rate" },
            { "Taxable", "Taxable" },
            { "LandedCost", "LandedCost" },
            { "Profitability", "Profitability" },
            { "SalesExecutive", "SalesExecutive" }
        };

                // Map incoming JSON data to the InvoiceDetails object
                foreach (var property in externalData.EnumerateObject())
                {
                    if (parameterMapping.TryGetValue(property.Name, out var mappedProperty))
                    {
                        switch (mappedProperty)
                        {
                            case "InvoiceNumber":
                                invoiceDto.InvoiceNumber = property.Value.GetString() ?? "Unknown";
                                break;
                            case "InvoiceDate":
                                invoiceDto.InvoiceDate = DateTime.TryParse(property.Value.GetString(), out var invoiceDate) ? invoiceDate : DateTime.Now;
                                break;
                            case "Customer":
                                invoiceDto.Customer = property.Value.GetString() ?? "Unknown";
                                break;
                            case "OfferNumber":
                                invoiceDto.OfferNumber = property.Value.GetString() ?? string.Empty;
                                break;
                            case "PartNumber":
                                invoiceDto.PartNumber = property.Value.GetString() ?? string.Empty;
                                break;
                            case "Qty":
                                invoiceDto.Qty = property.Value.ValueKind == JsonValueKind.Number ? property.Value.GetInt32() : 0;
                                break;
                            case "Rate":
                                invoiceDto.Rate = property.Value.ValueKind == JsonValueKind.Number ? property.Value.GetDecimal() : 0;
                                break;
                            case "Taxable":
                                invoiceDto.Taxable = property.Value.ValueKind == JsonValueKind.Number ? property.Value.GetDecimal() : 0;
                                break;
                            case "LandedCost":
                                invoiceDto.LandedCost = property.Value.ValueKind == JsonValueKind.Number ? property.Value.GetDecimal() : 0;
                                break;
                            case "Profitability":
                                invoiceDto.Profitability = property.Value.ValueKind == JsonValueKind.Number ? property.Value.GetDecimal() : 0;
                                break;
                            case "SalesExecutive":
                                invoiceDto.SalesExecutive = property.Value.GetString() ?? "Unknown";
                                break;
                        }
                    }
                }

                // Ensure all required properties are set
                if (string.IsNullOrEmpty(invoiceDto.InvoiceNumber))
                {
                    return BadRequest("Missing required parameter: InvoiceNumber.");
                }

                // Database operation
                using (SqlConnection conn = new SqlConnection(_Con.ConnectionString))
                {
                    using (var cmd = new SqlCommand("InsertInvoiceDetailss", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Add parameters dynamically based on the invoiceDto properties
                        cmd.Parameters.AddWithValue("@InvoiceNumber", invoiceDto.InvoiceNumber);
                        cmd.Parameters.AddWithValue("@InvoiceDate", invoiceDto.InvoiceDate);
                        cmd.Parameters.AddWithValue("@Customer", invoiceDto.Customer);
                        cmd.Parameters.AddWithValue("@OfferNumber", invoiceDto.OfferNumber);
                        cmd.Parameters.AddWithValue("@PartNumber", invoiceDto.PartNumber);
                        cmd.Parameters.AddWithValue("@Qty", invoiceDto.Qty);
                        cmd.Parameters.AddWithValue("@Rate", invoiceDto.Rate);
                        cmd.Parameters.AddWithValue("@Taxable", invoiceDto.Taxable);
                        cmd.Parameters.AddWithValue("@LandedCost", invoiceDto.LandedCost);
                        cmd.Parameters.AddWithValue("@Profitability", invoiceDto.Profitability);
                        cmd.Parameters.AddWithValue("@SalesExecutive", invoiceDto.SalesExecutive);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok("Invoice inserted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                // Ensure ID is valid
                if (id <= 0)
                {
                    return BadRequest("Invalid ID.");
                }

                InvoiceDetails invoice = null;

                // Fetch invoice by ID using a stored procedure
                using (SqlCommand cmd = new SqlCommand("GetInvoiceDetailsById", _Con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    _Con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            invoice = new InvoiceDetails
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                InvoiceNumber = reader["InvoiceNumber"].ToString(),
                                InvoiceDate = Convert.ToDateTime(reader["InvoiceDate"]),
                                Customer = reader["Customer"].ToString(),
                                OfferNumber = reader["OfferNumber"].ToString(),
                                PartNumber = reader["PartNumber"].ToString(),
                                Qty = Convert.ToInt32(reader["Qty"]),
                                Rate = Convert.ToDecimal(reader["Rate"]),
                                Taxable = Convert.ToDecimal(reader["Taxable"]),
                                LandedCost = Convert.ToDecimal(reader["LandedCost"]),
                                Profitability = Convert.ToDecimal(reader["Profitability"]),
                                SalesExecutive = reader["SalesExecutive"].ToString()
                            };
                        }
                    }

                    _Con.Close();
                }

               
                if (invoice == null)
                {
                    return NotFound($"Invoice with ID {id} not found.");
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open)
                {
                    _Con.Close();
                }
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        [HttpPut("{id:int}")]
        public IActionResult UpdateById(int id, [FromBody] InvoiceDetails updatedInvoice)
        {
            try
            {
                // Validate ID
                if (id <= 0)
                {
                    return BadRequest("Invalid ID.");
                }

                // Validate the input model
                if (updatedInvoice == null)
                {
                    return BadRequest("Invoice data is invalid.");
                }

                // Ensure the ID in the route matches the ID in the model
                updatedInvoice.Id = id;

                int rowsAffected;

                // Update invoice in the database using a stored procedure
                using (SqlCommand cmd = new SqlCommand("UpdateInvoiceDetailsById", _Con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the stored procedure
                    cmd.Parameters.AddWithValue("@Id", updatedInvoice.Id);
                    cmd.Parameters.AddWithValue("@InvoiceNumber", updatedInvoice.InvoiceNumber);
                    cmd.Parameters.AddWithValue("@InvoiceDate", updatedInvoice.InvoiceDate);
                    cmd.Parameters.AddWithValue("@Customer", updatedInvoice.Customer);
                    cmd.Parameters.AddWithValue("@OfferNumber", updatedInvoice.OfferNumber);
                    cmd.Parameters.AddWithValue("@PartNumber", updatedInvoice.PartNumber);
                    cmd.Parameters.AddWithValue("@Qty", updatedInvoice.Qty);
                    cmd.Parameters.AddWithValue("@Rate", updatedInvoice.Rate);
                    cmd.Parameters.AddWithValue("@Taxable", updatedInvoice.Taxable);
                    cmd.Parameters.AddWithValue("@LandedCost", updatedInvoice.LandedCost);
                    cmd.Parameters.AddWithValue("@Profitability", updatedInvoice.Profitability);
                    cmd.Parameters.AddWithValue("@SalesExecutive", updatedInvoice.SalesExecutive);

                    _Con.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                    _Con.Close();
                }

                if (rowsAffected > 0)
                {
                    return Ok("Invoice updated successfully.");
                }

                return NotFound($"Invoice with ID {id} not found.");
            }
            catch (Exception ex)
            {
                if (_Con.State == ConnectionState.Open)
                {
                    _Con.Close();
                }
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }













    }
}

    
