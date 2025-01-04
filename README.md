# CSC 13001 - Window Programming

---

# Course Project Report - MILESTONE 02

**Notion link for this report:**  [Notion – The all-in-one workspace for your notes, tasks, wikis, and databases.](https://www.notion.so/CSC-13001-Window-Programming-17030d59e498807baeefd425c3bbce5a?pvs=21)

## Team Members

| **No.** | **Student ID** | **Full Name** |
| --- | --- | --- |
| 1 | 22120303 | Mai Xuan Quy |
| 2 | 22120417 | Do Thi Anh Tuyet |
| 3 | 22120436 | Le Cao Tuan Vu |

## Completed Work

### Summary

- Implement generate invoices to pdf and csv files feature
- Implement hook to intercept keyboard and mouse click event:
    - In EmployeeUpdatePage, if user press Ctrl + S, the employee will be updated
    - If user right click, logout dialog will be shown
- Integrated the brand and category management within the Product page for streamlined operations.
- Report page
    - Display the top 10 products with the highest revenue and daily revenue reporting within a specified time range.
    - Utilized OxyPlot for graphical representation of revenue data.
- Dashboard page
    - Display the top employees based on the highest revenue within a specified time range, with the ability to adjust the time range settings as needed.
    - Show the total revenue for the current month.
    - Automatically send notifications when stock levels are running low (<5 units).

### Details.

- **Design Pattern and Structure**
    - **Structure**
        1. **Contracts**: Contains interfaces that define the contracts for services.
        2. **Core:** Contains models and enums, which are used for receive and send data to the server .
            1. **Branch, Category, Employee, Product**: These models represent the core entities of the application. They are used to map the data received from the server and to structure the data sent to the server.
            2. **Creation, Update, and Search Requests:** These models, such as **EmployeeCreationRequest**, **EmployeeUpdateRequest**, **ProductCreationRequest, and ProductSearchRequest**, are used to encapsulate the data required for creating and updating entities. They ensure that the necessary information is provided in a structured format when making API requests.
            3. **Enums:**  These are used to define a set of named constants, providing clarity and type safety for various properties in the models. For example, **UserRole** enum is used to specify the role of a user (e.g., **ADMIN, USER**).
            4. **ApiResponse, PageData, ErrorResponse**: These models apply generic types to handle various types of responses from the server.
                - **ApiResponse**: A generic wrapper for API responses, ensuring a consistent structure for all responses.
                - **PageData**: A generic model used for paginated data, containing properties like Page, Size, TotalElements, TotalPages, and a list of data items.
                - **ErrorResponse**: A model used to encapsulate error details returned by the server, providing a structured way to handle errors in the application.
        3. **Helpers:** Contains utility classes and methods that assist in various operations, such as validators and converters:
            1. **Converters:** These classes are used to convert data between different formats or types. For example, a **DoubleToCurrencyConverter** is used to handle the serialization and deserialization of Double type in a currency format.
            2. **Validators**: These classes are used to validate data before it is processed or sent to the server. For example, an **EmployeeCreationRequestValidator** is used to ensure that the data of new employee is valid before they are sent to the servet. Validators help maintain data integrity and prevent errors by ensuring that only valid data is processed.
        4. **Services**: Contains the implementation of service classes that handle business logic and interact with the server via HTTPS:
            1. **Services for Main Functionality**: These services handle the core business logic of the application, for examples:
                - **EmployeeService**: Manages operations related to employees, such as adding, updating, and retrieving employee information.
                - **AuthService**: Manages authentication-related operations, such as login, logout, and user registration.
            2. **Services for Helper Functionality**: These services provide auxiliary functions that support the main services.
                - **DialogService:** Manages the display of dialogs for user notifications, errors, and confirmations.
                - **HttpService**: Handles HTTP-related operations, such as add token to the header of requests and processing responses, ensuring consistent communication with the server.
    - **Design Patterns**
    Besides following the **MVVM** pattern mentioned in **Milestone 1**, our team has applied several other design patterns as outlined below.
        1. **Dependency Injection:** Dependency Injection (DI) is a design pattern used to implement Inversion of Control (IoC) between classes and their dependencies. It allows for the creation of dependent objects outside of a class and provides those objects to a class through various means: 
            - The project uses **DI** to manage dependencies between classes and their required components. For example, the **AuthService** class has dependencies on **HttpClient**, **IHttpService**, and **IDialogService**, which are injected through its constructor.
            - **Configuration: DI** is configured in the **App.xaml.cs** , where classes are registered and their lifetimes are defined (e.g., singleton,  transient).
        2. **Asynchronous Programming:** 
        The project extensively uses asynchronous programming to ensure non-blocking operations. Methods like **LoginAsync**, **GetAccountAsync**, **ChangePasswordAsync**, and **Register, …** are implemented as asynchronous methods using the async and await keywords.
        3. **Generic Programming:** 
        The project uses generic models like **PageData<T>** and **ApiResponse<T>** to handle paginated data and API responses, respectively. These models can work with any data type, making them highly reusable and flexible.
- **Advanced Features**
    1. **Role-Based Authorization:** 
        - The **AuthService** class handles authentication, then decodes the **role** from **JWT token** and stores it in local settings. This role information is then used to control access to various parts of the application.
            
            ```jsx
            public async Task<bool> LoginAsync(string username, string password)
            {
                // Authentication logic...
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token.Value);
                var userRole = jwtToken.Claims.FirstOrDefault(c => c.Type == "scope")?.Value;
            
                if (userRole != null)
                {
                    localSettings.Values["UserRole"] = userRole;
                }
            
                // Further logic...
            }
            
            ```
            
        - **Role Checks:** The application likely includes role checks in the UI and backend to ensure that users can only access resources and perform actions that their role permits, for examples:
            
            ```jsx
            private void OnMenuViewsReport()
            {
                UserRole userRole = _authService.GetUserRole();
                if (userRole != UserRole.ADMIN)
                {
                    _dialogService.ShowErrorAsync("Access Denied", "You do not have permission to access this page.");
                    return;
                }
                NavigationService.NavigateTo(typeof(ReportViewModel).FullName!);
            }
            
            private void OnMenuViewsCustomer()
            {
                UserRole userRole = _authService.GetUserRole();
                if (userRole != UserRole.ADMIN)
                {
                    _dialogService.ShowErrorAsync("Access Denied", "You do not have permission to access this page.");
                    return;
                }
                NavigationService.NavigateTo(typeof(CustomerViewModel).FullName!);
            }
            ```
            
        - Additionally, the **role-based authorization** is also imlemented on the backend
    2. **Calling API Through HTTP:** 
        
        Calling **APIs** through **HTTP** is used for interacting with backend services. It involves sending HTTP requests and processing the responses to perform CRUD operations and other actions.
        
    3. **Generic classes:**
    The project uses generic models like **PageData<T>** and **ApiResponse<T>** to handle paginated data and API responses, respectively. These models can work with any data type, making them highly reusable and flexible.
    4. **Enum Types: 
    Enums** are used to define a set of named constants, providing clarity and type safety for various properties in the models.
        - **UserRole**: An enum used to specify the role of a user (e.g., ADMIN, USER).
        - **SortType:** An enum used to specify the type of sorting (e.g., ASC, DESC).
    5. **Converters in Helpers**
        
        **Converters** are used to transform data between different formats or types, often for serialization and deserialization purposes.
        
        - **BooleanToVisibilityConverter:** This converter is used in XAML to bind the visibility of UI elements to boolean properties in the ViewModel. For example, the **Unemployed** **Button** can be shown or hidden based on the employee’s status.
            
            ```jsx
            public class BooleanToVisibilityConverter : IValueConverter
            {
                public object Convert(object value, Type targetType, object parameter, string language)
                {
                    if (value is bool boolValue)
                    {
                        bool invert = parameter != null && bool.TryParse(parameter.ToString(), out bool paramValue) && paramValue;
                        return (boolValue ^ invert) ? Visibility.Visible : Visibility.Collapsed;
                    }
                    return Visibility.Visible;
                }
            
                public object ConvertBack(object value, Type targetType, object parameter, string language)
                {
                    throw new NotSupportedException("BooleanToVisibilityConverter does not support ConvertBack.");
                }
            }
            ```
            
            ```xml
            <!-- Unemployed Button with Icon -->
            <Button Margin="10" Width="Auto" Height="35" Click="MarkUnemployedButton_Click" Background="#ED232F" FontWeight="SemiBold" Foreground="White"
            Visibility="{Binding CurrentEmployee.EmploymentStatus, Converter={StaticResource BooleanToVisibilityConverter}}">
                <StackPanel Orientation="Horizontal">
                    <FontIcon Glyph="&#xE72E;" FontSize="16" Margin="0,0,10,0" />
                    <!-- Unemployed icon (Person) -->
                    <TextBlock Text="Unemployed" />
                </StackPanel>
            </Button>
            ```
            
        - **DoubleToCurrencyConverter:** This converter is used in XAML to bind double properties in the ViewModel to text elements in the UI, displaying them as currency and converting back when collecting data .
            
            ```csharp
            public class DoubleToCurrencyConverter : IValueConverter
            {
                public object Convert(object value, Type targetType, object parameter, string language)
                {
                    if (value is double doubleValue)
                    {
                        return doubleValue.ToString("N", CultureInfo.CurrentCulture);
                    }
                    return value;
                }
            
                public object ConvertBack(object value, Type targetType, object parameter, string language)
                {
                    if (value is string strValue && double.TryParse(strValue, NumberStyles.Number, CultureInfo.CurrentCulture, out double result))
                    {
                        return result;
                    }
                    return value;
                }
            }
            ```
            
        - **StatusesConverter: EmploymentStatusConverter and BusinessStatusConverter** are used to transform status values into user-friendly strings or other representations suitable for display in the UI. These converters enhance the readability and usability of the application by providing meaningful representations of status codes.
            
            ```csharp
            public class BusinessStatusConverter : IValueConverter
            {
                public object Convert(object value, Type targetType, object parameter, string language)
                {
                    if (value is bool boolValue)
                    {
                        return boolValue ? "Active" : "Inactive"; // True -> Employed, False -> Resigned
                    }
                    return "Resigned"; // Default to "Resigned" if not a boolean
                }
            
                public object ConvertBack(object value, Type targetType, object parameter, string language)
                {
                    if (value is string strValue)
                    {
                        return strValue == "Active"; // Convert "Employed" back to true, "Resigned" to false
                    }
                    return false; // Default to false if not "Employed"
                }
            }
            
            public class EmploymentStatusConverter : IValueConverter
            {
                public object Convert(object value, Type targetType, object parameter, string language)
                {
                    if (value is bool boolValue)
                    {
                        return boolValue ? "Employed" : "Resigned"; // True -> Employed, False -> Resigned
                    }
                    return "Resigned"; // Default to "Resigned" if not a boolean
                }
            
                public object ConvertBack(object value, Type targetType, object parameter, string language)
                {
                    if (value is string strValue)
                    {
                        return strValue == "Employed"; // Convert "Employed" back to true, "Resigned" to false
                    }
                    return false; // Default to false if not "Employed"
                }
            }
            ```
            
        - **DateTimeConverter:** is a custom `JsonConverter` for handling `DateTimeOffset` objects during JSON serialization and deserialization. This custom converter ensures that `DateTimeOffset` objects are serialized and deserialized in a specific format (with just the date, excluding time) during JSON operations.
            
            ```csharp
            public class DateTimeConverter : JsonConverter<DateTimeOffset>
            {
            /// <summary>
            /// Reads and converts the JSON to <see cref="DateTimeOffset"/>.
            /// </summary>
            /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
            /// <param name="typeToConvert">The type to convert.</param>
            /// <param name="options">Options to control the conversion behavior.</param>
            /// <returns>The converted <see cref="DateTimeOffset"/>.</returns>
            public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
            return DateTimeOffset.Parse(reader.GetString());
            }
            
            /// <summary>
            /// Writes a <see cref="DateTimeOffset"/> as a JSON string.
            /// </summary>
            /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
            /// <param name="value">The value to convert.</param>
            /// <param name="options">Options to control the conversion behavior.</param>
            public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
            }
            public class DateTimeConverter : JsonConverter<DateTimeOffset>
            {
                public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    return DateTimeOffset.Parse(reader.GetString());
                }
            
                public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
                {
                    writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
                }
            }
            ```
            
        - **DateOnlyToDateTimeOffsetConverter:** implements the `IValueConverter` interface and provides conversion logic for transforming between `DateOnly` and `DateTimeOffset` types. This converter is useful when working with `DateOnly` and `DateTimeOffset` types in UI-bound data scenarios, ensuring proper conversion between these types during data binding operations.
            
            ```jsx
            public class DateOnlyToDateTimeOffsetConverter : IValueConverter
            {
                
                public object Convert(object value, Type targetType, object parameter, string language)
                {
                    if (value is DateOnly date)
                    {
                        // Convert DateOnly to DateTimeOffset by creating a DateTime with the minimum time.
                        return new DateTimeOffset(date.ToDateTime(TimeOnly.MinValue));
                    }
                    return value; // Returns the original value if it is not a DateOnly.
                }
            
               
                public object ConvertBack(object value, Type targetType, object parameter, string language)
                {
                    if (value is DateTimeOffset dateTimeOffset)
                    {
                        // Convert DateTimeOffset back to DateOnly by extracting the Date part of the DateTime.
                        return DateOnly.FromDateTime(dateTimeOffset.DateTime);
                    }
                    return value; // Returns the original value if it is not a DateTimeOffset.
                }
            }
            ```
            
    6. **File exporters**
        - **PdfExporter**
            
            The **PdfExporter** class is responsible for exporting invoice data to a PDF file. It uses the **Syncfusion PDF library** to create and format the PDF document.
            
            **ExportInvoiceToPdf(Invoice invoice, string filePath)**: This method takes an Invoice object and a file path as parameters. It creates a PDF document with the invoice details, including the invoice ID, employee name, created date, total amount, real amount, and a table of invoice details.
            
        - **CsvExporter**
            
            The **CsvExporter** class is responsible for exporting a list of invoices to a CSV file. It uses the **EPPlus library** to create and format the CSV file.
            
            **ExportInvoicesToCsv(List<Invoice> invoices, string filePath):** This method takes a list of Invoice objects and a file path as parameters. It creates a CSV file with the invoice details, including the invoice ID, employee name, created date, total amount, and real amount.
            
    7. **Hook**
        - **GlobalKeyBoardHook**
            - **Purpose**: Captures global keyboard events, including key presses and modifier states (e.g., Ctrl key).
            - **Key Features**:
                - Utilizes the `SetWindowsHookEx` function with `WH_KEYBOARD_LL` to install a low-level keyboard hook.
                - Handles the `WM_KEYDOWN` message to detect key press events.
                - Determines if the Ctrl key is pressed using `GetKeyState(VK_CONTROL)`.
                - Exposes an event, `KeyPressed`, which provides the key code and Ctrl state to subscribers.
            - **Use Case**: Intercept "Ctrl+S" for automatically updade employee.
        - **GlobalMouseHook**
            - **Purpose**: Captures global mouse events, specifically left and right mouse button clicks.
            - **Key Features**:
                - Utilizes the `SetWindowsHookEx` function with `WH_MOUSE_LL` to install a low-level mouse hook.
                - Handles the `WM_LBUTTONDOWN` and `WM_RBUTTONDOWN` messages to detect left and right mouse clicks.
                - Exposes an event, `MouseClickDetected`, which notifies subscribers of the type of mouse click ("Left" or "Right").
            - **Use Case**: Intercept right mouse clicks to show logout dialog
    8. **Advanced Charting with OxyPlot**
        
        Developed dynamic bar charts using `BarSeries` in OxyPlot to visualize top product revenues and daily revenue trends, with data sourced in real-time from our REST API.
        
- **Quality Assurance**
    - Created unit tests to test features before merging into main source code.
    - Evidence screenshots: [https://drive.google.com/drive/folders/1aK_0RVFv45kY1vKge6uFNpECG_CVpPuW?usp=sharing](https://drive.google.com/drive/folders/1aK_0RVFv45kY1vKge6uFNpECG_CVpPuW?usp=sharing)

## Team work

1. **Tools Used**
    - Messenger, Notion
2. **Work Process**
    - Team meets regularly at 9 PM every Monday for progress reports and task distribution.
    - Project documentation such as Database schema, features, workflow, and resources are posted on the team's dedicated Notion link.
        - Notion link: [Notion – The all-in-one workspace for your notes, tasks, wikis, and databases.](https://www.notion.so/Project-Docs-11530d59e49880a6a8ffda0d8df2c4ff?pvs=21)
    - Meeting minutes: [Google Docs](https://docs.google.com/document/d/1PCO1waWsLK8V03GiTuQv9KtMi7uyXxYTcoN9CwUKMBE/edit?usp=sharing)
3. **Working with Git**
    - Team members commit code to a shared repository.
    - After features are completed, they are tested. After passing tests, members create pull requests for other members to review. Once approved, code is merged into the main branch.

## **Sources**

- Repository link: [vuhoabinhthachhoa/WindowProgramming](https://github.com/vuhoabinhthachhoa/WindowProgramming)

## Self-Assessment

| **No.** | **Student ID** | **Full Name** | **Working Hours** |
| --- | --- | --- | --- |
| 1 | 22120303 | Mai Xuan Quy | 4.5 |
| 2 | 22120417 | Do Thi Anh Tuyet | 4.5 |
| 3 | 22120436 | Le Cao Tuan Vu | 4.5 |
