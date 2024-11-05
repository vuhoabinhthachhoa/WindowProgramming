package com.windowprogramming.ClothingStoreManager.dto.request.branch;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class BranchCreationRequest {
    @NotBlank(message = "REQUIRED_BRANCH_NAME")
    String name;
}
