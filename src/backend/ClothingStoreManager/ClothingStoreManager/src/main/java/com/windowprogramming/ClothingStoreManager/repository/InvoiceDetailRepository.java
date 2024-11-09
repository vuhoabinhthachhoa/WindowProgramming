package com.windowprogramming.ClothingStoreManager.repository;

import com.windowprogramming.ClothingStoreManager.entity.Invoice;
import com.windowprogramming.ClothingStoreManager.entity.InvoiceDetail;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface InvoiceDetailRepository extends JpaRepository<InvoiceDetail, Long> {
    List<InvoiceDetail> findAllByInvoice(Invoice invoice);

    void deleteAllByInvoice(Invoice invoice);
}
