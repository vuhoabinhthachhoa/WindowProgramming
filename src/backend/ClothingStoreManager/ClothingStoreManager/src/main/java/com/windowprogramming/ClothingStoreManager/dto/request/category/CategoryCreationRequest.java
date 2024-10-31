package com.windowprogramming.ClothingStoreManager.dto.request.category;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import lombok.AccessLevel;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class CategoryCreationRequest {
    @NotBlank(message = "REQUIRED_CATEGORY_ID")
    String id;

    @NotBlank(message = "REQUIRED_CATEGORY_NAME")
    String name;
}
