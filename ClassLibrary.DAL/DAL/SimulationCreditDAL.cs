using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ClassLibrary.BO.ModelNotConnectDB;
using ClassLibrary.DAL.Interfaces;
using Microsoft.Data.SqlClient;

namespace ClassLibrary.DAL.DAL
{
    public class SimulationCreditDAL : ISimulationCredit
    {
        private readonly string _connectionString;

        public SimulationCreditDAL(string connectionString) // Bisa modifikasi konstruktor untuk terima connection string
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<CarSimulationCredit>> GetCarSimulationCreditsAsync(CarInputSimulationCredit carInputSimulationCredit)
        {
            var results = new List<CarSimulationCredit>();

            string sqlQuery = "SELECT * FROM dbo.SimulateCarLoanSimple(@DealerId, @CarId, @DownPayment, @LoanTermMonths, @AnnualInterestRate)";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@DealerId", carInputSimulationCredit.DealerID);
                    command.Parameters.AddWithValue("@CarId", carInputSimulationCredit.CarID);
                    command.Parameters.AddWithValue("@DownPayment", carInputSimulationCredit.DownPayment);
                    command.Parameters.AddWithValue("@LoanTermMonths", carInputSimulationCredit.TermMonths);
                    command.Parameters.AddWithValue("@AnnualInterestRate", carInputSimulationCredit.AnnualInterestRate);

                    await connection.OpenAsync();

                    Console.WriteLine("Executing SQL Command: " + command.CommandText);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(new CarSimulationCredit
                            {
                                DealerCarID = reader.GetInt32(reader.GetOrdinal("DealerCarID")),
                                DealerPrice = Convert.ToDouble(reader["DealerPrice"]),
                                DealerName = reader.GetString(reader.GetOrdinal("DealerName")),
                                CarModel = reader.GetString(reader.GetOrdinal("CarModel")),
                                Tax = Convert.ToDouble(reader["Tax"]),
                                PriceAfterTax = Convert.ToDouble(reader["PriceAfterTax"]),
                                DownPayment = Convert.ToDouble(reader["DownPayment"]),
                                LoanAmount = Convert.ToDouble(reader["LoanAmount"]),
                                TermMonths = reader.GetInt32(reader.GetOrdinal("TermMonths")),
                                AnnualInterestRate = Convert.ToSingle(reader["AnnualInterestRate"]),
                                MonthlyPrincipal = Convert.ToDouble(reader["MonthlyPrincipal"]),
                                TotalInterest = Convert.ToDouble(reader["TotalInterest"]),
                                MonthlyInterest = Convert.ToDouble(reader["MonthlyInterest"]),
                                MonthlyPayment = Convert.ToDouble(reader["MonthlyPayment"])
                            });
                        }
                    }
                }

                Console.WriteLine("Query executed successfully. Number of results: " + results.Count);

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing query: " + ex.Message);
                // Jangan lupa untuk logging sebenarnya
                throw new InvalidOperationException("Error retrieving car simulation credits: " + ex.Message, ex);
            }
        }
    }
}
