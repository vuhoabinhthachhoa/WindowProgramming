package com.windowprogramming.ClothingStoreManager.entity;

import com.windowprogramming.ClothingStoreManager.enums.RoleName;
import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Entity(name = "roles")
@Table(name = "roles")
@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class Role {
    @Id
    @Enumerated(EnumType.STRING)
    @Column(name = "role_name")
    RoleName name;
    @Column(name = "description")
    String description;

}
