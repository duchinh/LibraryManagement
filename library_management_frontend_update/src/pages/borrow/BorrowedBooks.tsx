import React, { useState, useEffect } from 'react';
import { Table, Tag, Button, message } from 'antd';
import { Book } from '../../interfaces';
import { bookService } from '../../services/book.service';

const BorrowedBooks: React.FC = () => {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    fetchBorrowedBooks();
  }, []);

  const fetchBorrowedBooks = async () => {
    try {
      setLoading(true);
      const response = await bookService.getBorrowedBooks();
      setBooks(response.data);
    } catch (error) {
      message.error('Không thể tải danh sách sách đã mượn');
    } finally {
      setLoading(false);
    }
  };

  const handleReturn = async (bookId: string) => {
    try {
      await bookService.returnBook(bookId);
      message.success('Trả sách thành công');
      fetchBorrowedBooks();
    } catch (error) {
      message.error('Trả sách thất bại');
    }
  };

  const getStatusTag = (status: string) => {
    switch (status) {
      case 'Borrowed':
        return <Tag color="blue">Đang mượn</Tag>;
      case 'Overdue':
        return <Tag color="red">Quá hạn</Tag>;
      default:
        return <Tag>{status}</Tag>;
    }
  };

  const columns = [
    {
      title: 'Tiêu đề',
      dataIndex: 'title',
      key: 'title',
    },
    {
      title: 'Tác giả',
      dataIndex: 'author',
      key: 'author',
    },
    {
      title: 'Danh mục',
      dataIndex: 'categoryName',
      key: 'categoryName',
    },
    {
      title: 'Ngày mượn',
      dataIndex: 'borrowDate',
      key: 'borrowDate',
    },
    {
      title: 'Hạn trả',
      dataIndex: 'dueDate',
      key: 'dueDate',
    },
    {
      title: 'Trạng thái',
      dataIndex: 'status',
      key: 'status',
      render: (status: string) => getStatusTag(status),
    },
    {
      title: 'Thao tác',
      key: 'action',
      render: (text: string, record: Book) => (
        <Button type="primary" onClick={() => handleReturn(record.id)}>
          Trả sách
        </Button>
      ),
    },
  ];

  return (
    <div>
      <Table
        dataSource={books}
        columns={columns}
        rowKey="id"
        loading={loading}
      />
    </div>
  );
};

export default BorrowedBooks; 