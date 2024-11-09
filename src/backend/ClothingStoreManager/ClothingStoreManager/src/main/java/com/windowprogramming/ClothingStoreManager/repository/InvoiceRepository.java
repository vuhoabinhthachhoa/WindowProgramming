package com.windowprogramming.ClothingStoreManager.repository;

import com.windowprogramming.ClothingStoreManager.entity.Invoice;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.time.LocalDate;
import java.util.List;

@Repository
public interface InvoiceRepository extends JpaRepository<Invoice, Long> {
    List<Invoice> findAllByCreatedDateBetween(LocalDate startDate, LocalDate endDate);
    List<Invoice> findAllByCreatedDateBefore(LocalDate endDate);
    List<Invoice> findAllByCreatedDateAfter(LocalDate startDate);


    Page<Invoice> findAllByCreatedDateBetween(LocalDate startDate, LocalDate endDate, Pageable pageable);

    Page<Invoice> findAllByCreatedDateBefore(LocalDate endDate, Pageable pageable);

    Page<Invoice> findAllByCreatedDateAfter(LocalDate startDate, Pageable pageable);
}
