using System.Text;
using Capstone.Application.Admin.Commands.CreateRescue;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Capstone.Api.Tests.Admin;
public class CreateRescueTest : BaseUnitTest
{
    private const string TOKEN = "eyJhbGciOiJSUzI1NiIsImtpZCI6IkM5NDM1RjJDRTU1MDFBNENCRkY0RTM1MUVDRERGRjRFIiwidHlwIjoiYXQrand0In0.eyJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMzYiLCJuYmYiOjE3NDEyNDc0NTMsImlhdCI6MTc0MTI0NzQ1MywiZXhwIjoxNzQxMjUxMDUzLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUwMzYvcmVzb3VyY2VzIiwic2NvcGUiOlsiZW1haWwiLCJvcGVuaWQiLCJwcm9maWxlIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbInB3ZCJdLCJjbGllbnRfaWQiOiJtYWdpYyIsInN1YiI6IjAzMGNlOWM2LWM1YTEtNDI3Yi1iMzIyLWI5MTM4ZjA2NjkwNSIsImF1dGhfdGltZSI6MTc0MTI0NzQ1MiwiaWRwIjoibG9jYWwiLCJpZCI6ImQ4MmFmMjEwLTZjNjktNDA3My05ZjhlLWU5ZGQwZDkyZmFlZSIsInJvbGUiOiJBZG1pbiIsImp0aSI6IkFCRkVCQjlCQ0FEQjgxQjc1ODQ0MzZFMDJFMDUyRjZFIn0.o4n5dgY5-7CpqNkouH6sn53P9OYI5COrmRsFdBI8LXkOjRrWTG8ZtsUH97Fl23gbi3HG_eyAQ5PSMc_DauI4pdILgmsAw25r7Kd-xmG1zrTI4h04NuOb1zBGXeCHKsCjQuWvT9vDH7zjSnUFNRgbfcGiJnkzKOxOynD4MB2-I5VvyJkE1IjK3s5GXnU1qbVn1FwSBF1L3mcbs7f-7d1PO9JIo-Z4q-RkqrWmXJ16N2Y0w-B0vnRZ40g6Zom_pWtQB3g8sVNLV7_B7y_PJqSeF13n3CGCGMu1Nm4e_3fJDHAImpV7HoNWtYo7-8gP0BM7-4ybU1c_3CF0IGWAmk63sw";
    private const string INCORRECT_TOKEN = "";
    [TestCaseSource(nameof(ListTestCaseCreated))]
    public async Task CreateRescue_Test(
        string token,
        string dataTestCase,
        string expectedDataFilePath,
        Type expectedDataType
    )
    {
        //await InitDbTestAsync(Path.Combine("CreateRescueTest", "DbContext"));

        var createRescueDto = GetBodyRequest(dataTestCase);

        var expected = LoadExpectedDataFromFile(expectedDataType, Path.Combine("CreateRescueTest", "ExpectedData", expectedDataFilePath));

        var url = "admin/rescues";

        var content = new StringContent(JsonConvert.SerializeObject(createRescueDto), Encoding.UTF8, "application/json");

        HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var res = await HttpClient.PostAsync(url, content);

        var resContentStr = await res.Content.ReadAsStringAsync();

        var actual = JsonConvert.DeserializeObject(resContentStr, expectedDataType);

        actual.IsStructuralEqual(expected);
    }
    private static IEnumerable<TestCaseData> ListTestCaseValidate
    {
        get
        {
            yield return new TestCaseData
            ();
        }
    }
    private static IEnumerable<TestCaseData> ListTestCaseCreated
    {
        get
        {
            yield return new TestCaseData(
                TOKEN,
                nameof(DataTestCase.CreateSuccess),
                "CreateSuccess.json",
                typeof(CreateRescueResult)
            );
        }
    }
    private enum DataTestCase
    {
        CreateSuccess
    }
    private static object? GetBodyRequest(string dataTestCase)
    {
        var dataRoot = dataTestCase switch
        {
            nameof(DataTestCase.CreateSuccess) => "CreateSuccess.json",
            _ => string.Empty
        };

        return LoadExpectedDataFromFile(typeof(object), Path.Combine("CreateRescueTest", "RequestData", dataRoot));
    }
}