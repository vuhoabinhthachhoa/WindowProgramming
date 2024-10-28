package com.windowprogramming.ClothingStoreManager.mapper;

import com.windowprogramming.ClothingStoreManager.dto.request.authentication.RegistrationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.authentication.UserUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.UserResponse;
import com.windowprogramming.ClothingStoreManager.entity.User;
import org.mapstruct.Mapper;
import org.mapstruct.Mapping;
import org.mapstruct.MappingTarget;
import org.mapstruct.NullValuePropertyMappingStrategy;

@Mapper(componentModel = "spring", nullValuePropertyMappingStrategy = NullValuePropertyMappingStrategy.IGNORE)
public interface UserMapper {

    // specify which fields of the target object should be ignored
    @Mapping(target = "role", ignore = true)
    @Mapping(target = "employee", ignore = true)
    User toUser(RegistrationRequest request);

    @Mapping(target = "role", ignore = true)
    @Mapping(target = "employeeId", ignore = true)
    UserResponse toUserResponse(User user);

    @Mapping(target = "role", ignore = true)
    @Mapping(target = "employee", ignore = true)
    void updateUser(@MappingTarget User user, UserUpdateRequest request);
}
