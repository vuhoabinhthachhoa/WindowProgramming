package com.windowprogramming.ClothingStoreManager.service.authentication;

import com.windowprogramming.ClothingStoreManager.dto.request.authentication.*;
import com.windowprogramming.ClothingStoreManager.dto.response.LoginResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.UserResponse;

import java.util.List;

public interface AuthenticationService {
    LoginResponse login(LoginRequest loginRequest);
    UserResponse register(RegistrationRequest registrationRequest);
    void logout(LogoutRequest logoutRequest);
    void changePassword(ChangePasswordRequest changePasswordRequest);
    void deleteUser(Long id);
    UserResponse getUserById(Long id);
    UserResponse updateUser(UserUpdateRequest userUpdateRequest);
    List<UserResponse> getAllUsers();
}
