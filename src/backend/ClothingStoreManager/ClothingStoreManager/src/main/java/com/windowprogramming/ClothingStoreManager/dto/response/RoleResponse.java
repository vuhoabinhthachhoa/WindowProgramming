package com.windowprogramming.ClothingStoreManager.dto.response;

import com.windowprogramming.ClothingStoreManager.enums.RoleName;
import lombok.AccessLevel;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.FieldDefaults;

import java.util.List;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class RoleResponse {
    RoleName name;
    String description;;
}
