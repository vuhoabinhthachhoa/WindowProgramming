package com.windowprogramming.ClothingStoreManager.service.authentication;

import com.windowprogramming.ClothingStoreManager.dto.request.authentication.*;
import com.windowprogramming.ClothingStoreManager.dto.response.LoginResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.UserResponse;
import com.windowprogramming.ClothingStoreManager.entity.Employee;
import com.windowprogramming.ClothingStoreManager.entity.Role;
import com.windowprogramming.ClothingStoreManager.entity.User;
import com.windowprogramming.ClothingStoreManager.enums.RoleName;
import com.windowprogramming.ClothingStoreManager.exception.AppException;
import com.windowprogramming.ClothingStoreManager.exception.ErrorCode;
import com.windowprogramming.ClothingStoreManager.mapper.EmployeeMapper;
import com.windowprogramming.ClothingStoreManager.mapper.RoleMapper;
import com.windowprogramming.ClothingStoreManager.mapper.UserMapper;
import com.windowprogramming.ClothingStoreManager.repository.EmployeeRepository;
import com.windowprogramming.ClothingStoreManager.repository.RoleRepository;
import com.windowprogramming.ClothingStoreManager.repository.UserRepository;
import com.windowprogramming.ClothingStoreManager.utils.JWTUtils;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.data.jpa.repository.support.SimpleJpaRepository;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class AuthenticationServiceImpl implements AuthenticationService {
    UserRepository userRepository;
    RoleRepository roleRepository;
    EmployeeRepository employeeRepository;

    PasswordEncoder passwordEncoder;
    JWTUtils jwtUtils;
    UserMapper userMapper;
    RoleMapper roleMapper;
    EmployeeMapper employeeMapper;


    @Override
    public LoginResponse login(LoginRequest loginRequest) {
        User user = userRepository.findByUsername(loginRequest.getUsername())
                .orElseThrow(() -> new AppException(ErrorCode.INVALID_USERNAME_PASSWORD));

        if (!passwordEncoder.matches(loginRequest.getPassword(), user.getPassword())) {
            throw new AppException(ErrorCode.INVALID_USERNAME_PASSWORD);
        }

        return buildLoginResponse(user);
    }

    private LoginResponse buildLoginResponse(User user) {
        return LoginResponse.builder()
                .token(jwtUtils.generateToken(user))
                .build();
    }

    private UserResponse buildUserResponse(User user){
        UserResponse userResponse = userMapper.toUserResponse(user);
        userResponse.setRole(roleMapper.toRoleResponse(user.getRole()));
        userResponse.setEmployee((user.getEmployee() != null) ? employeeMapper.toEmployeeResponse(user.getEmployee()) : null);
        return userResponse;
    }

    @Override
    public UserResponse register(RegistrationRequest registrationRequest) {
        if (userRepository.existsByUsername(registrationRequest.getUsername())) {
            throw new AppException(ErrorCode.USER_EXISTED);
        }

        User user = userMapper.toUser(registrationRequest);

        Role role = roleRepository.findById(registrationRequest.getRole())
                .orElseThrow(() -> new AppException(ErrorCode.ROLE_NOT_FOUND));
        user.setRole(role);

        Employee employee = null;
        if(registrationRequest.getRole() == RoleName.USER) {
            if(registrationRequest.getEmployeeId() == null) {
                throw new AppException(ErrorCode.REQUIRED_EMPLOYEE_ID);
            }
            else {
                employee = employeeRepository.findById(registrationRequest.getEmployeeId())
                        .orElseThrow(() -> new AppException(ErrorCode.EMPLOYEE_NOT_FOUND));
            }
        }
        user.setEmployee(employee);

        user.setPassword(passwordEncoder.encode(user.getPassword()));

        user = userRepository.save(user);

        return buildUserResponse(user);
    }

    @Override
    public void logout(LogoutRequest logoutRequest) {
        // do not need to implement logout
    }

    @Override
    public void changePassword(ChangePasswordRequest request) {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        User user = userRepository.findByUsername(authentication.getName())
                .orElseThrow(() -> new AppException(ErrorCode.USER_NOT_FOUND));

        if (!passwordEncoder.matches(request.getOldPassword(), user.getPassword())) {
            throw new AppException(ErrorCode.INCORRECT_PASSWORD);
        }

        if (request.getOldPassword().equals(request.getNewPassword())) {
            throw new AppException(ErrorCode.SAME_PASSWORD);
        }

        user.setPassword(passwordEncoder.encode(request.getNewPassword()));
        userRepository.save(user);
    }

    @Override
    public void deleteUser(Long id) {
        if(userRepository.findById(id).isEmpty()) {
            throw new AppException(ErrorCode.USER_NOT_FOUND);
        }
        userRepository.deleteById(id);
    }

    @Override
    public UserResponse getUserById(Long id) {
        User user = userRepository.findById(id)
                .orElseThrow(() -> new AppException(ErrorCode.USER_NOT_FOUND));

        return buildUserResponse(user);
    }

    @Override
    public UserResponse updateUser(UserUpdateRequest userUpdateRequest) {
        User user = userRepository.findById(userUpdateRequest.getId())
                .orElseThrow(() -> new AppException(ErrorCode.USER_NOT_FOUND));
        userMapper.updateUser(user, userUpdateRequest);
        userRepository.save(user);
        return buildUserResponse(user);
    }

    @Override
    public List<UserResponse> getAllUsers() {
        List<User> users = userRepository.findAll();
        List<UserResponse> userResponses = new ArrayList<>();
        for(User user : users) {
            userResponses.add(buildUserResponse(user));
        }
        return userResponses;
    }
}
