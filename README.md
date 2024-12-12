# CSC 13001 - Window Programming

---

# Course Project Report - MILESTONE 02

**Notion link for this report:** [CSC 13001 - Window Programming](https://quiver-frog-6c1.notion.site/CSC-13001-Window-Programming-15930d59e49880edb409cfa12e10ac0d?pvs=74)

## Team Members

| **No.** | **Student ID** | **Full Name** |
| --- | --- | --- |
| 1 | 22120303 | Mai Xuan Quy |
| 2 | 22120417 | Do Thi Anh Tuyet |
| 3 | 22120436 | Le Cao Tuan Vu |

## Completed Work

### Summary

- **Backend**:
    - Complete all features and deploy on [Railway.com](http://railway.com/)
- **Frontend**:
    - Migrate **Employee**, **Product**, **Account**, and **Sale** features from mock data to real data on the server.
        - **Employee**:  Add, edit, search, and display employees with pagination
        - **Product:** Add, edit, search, and display products with pagination. Additionally, display product images in high quality and allow users to upload images from their devices.
        - **Account:** display, update account infomation, change password
        - **Sale:** Search product and add it to the order, apply discount and calculate the amount.
    - Improve the UI for better aesthetics.
    - Correct shortcomings in the design pattern structure:
        - Remove hard-coded values.
        - Apply advanced design patterns.

### Details

- **UI/UX**
    - Add icons to buttons.
    - Change the colors of the background, header, and buttons to enhance visual appeal.
    - Add padding and margin to input fields for a cleaner layout.
    - Validates user input for missing data and incorrect formats.
    - Use **DialogService** to display notifications.
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
            
- **Quality Assurance**
    - Created unit tests to test features before merging into main source code.
    - Evidence screenshots: [https://drive.google.com/drive/folders/172W6vXROOqYkkvB5pYm5p7guMshf04Di?usp=sharing](https://drive.google.com/drive/folders/172W6vXROOqYkkvB5pYm5p7guMshf04Di?usp=sharing)

## Team work

1. **Tools Used**
    - Messenger, Notion
2. **Work Process**
    - Team meets regularly at 9 PM every Monday for progress reports and task distribution.
    - Project documentation such as Database schema, features, workflow, and resources are posted on the team's dedicated Notion link.
        - Notion link: [Notion – The all-in-one workspace for your notes, tasks, wikis, and databases.](https://quiver-frog-6c1.notion.site/Project-Docs-11530d59e49880a6a8ffda0d8df2c4ff?pvs=74)
    - Meeting minutes: [Google Docs](https://docs.google.com/document/d/1PCO1waWsLK8V03GiTuQv9KtMi7uyXxYTcoN9CwUKMBE/edit?usp=sharing)
3. **Working with Git**
    - Team members commit code to a shared repository.
    - After features are completed, they are tested. After passing tests, members create pull requests for other members to review. Once approved, code is merged into the main branch.

## **Sources**

- Repository link: [vuhoabinhthachhoa/WindowProgramming](https://github.com/vuhoabinhthachhoa/WindowProgramming)
- Video: [(16) WP Project - Milestone 1 - Demo - YouTube](https://www.youtube.com/watch?v=RH5zzlCKwNA&feature=youtu.be)

## Self-Assessment

| **No.** | **Student ID** | **Full Name** | **Working Hours** |
| --- | --- | --- | --- |
| 1 | 22120303 | Mai Xuan Quy | 4.5 |
| 2 | 22120417 | Do Thi Anh Tuyet | 4.5 |
| 3 | 22120436 | Le Cao Tuan Vu | 4.5 |
