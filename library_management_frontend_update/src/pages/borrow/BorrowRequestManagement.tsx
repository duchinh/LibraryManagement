import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, message, Tag } from 'antd';
import { CheckOutlined, CloseOutlined } from '@ant-design/icons';
import { api } from '../../constants/api';
import { BorrowRequest } from '../../interfaces/borrow.interface';
import { borrowRequestService } from '../../services/borrow-request.service';

const BorrowRequestManagement: React.FC = () => {
  const [requests, setRequests] = useState<BorrowRequest[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    fetchRequests();
  }, []);

  const fetchRequests = async () => {
    try {
      setLoading(true);
      const response = await borrowRequestService.getAllRequests();
      setRequests(response.data);
    } catch (error) {
      message.error('Không thể tải danh sách yêu cầu');
    } finally {
      setLoading(false);
    }
  };

  const handleApprove = async (id: string) => {
    Modal.confirm({
      title: 'Xác nhận duyệt',
      content: 'Bạn có chắc chắn muốn duyệt yêu cầu này?',
      onOk: async () => {
        try {
          await borrowRequestService.approveRequest(id);
          message.success('Duyệt yêu cầu thành công');
          fetchRequests();
        } catch (error) {
          message.error('Duyệt yêu cầu thất bại');
        }
      },
    });
  };

  const handleReject = async (id: string) => {
    Modal.confirm({
      title: 'Xác nhận từ chối',
      content: 'Bạn có chắc chắn muốn từ chối yêu cầu này?',
      onOk: async () => {
        try {
          await borrowRequestService.rejectRequest(id);
          message.success('Từ chối yêu cầu thành công');
          fetchRequests();
        } catch (error) {
          message.error('Từ chối yêu cầu thất bại');
        }
      },
    });
  };

  const getStatusTag = (status: string) => {
    switch (status) {
      case 'Pending':
        return <Tag color="orange">Đang chờ</Tag>;
      case 'Approved':
        return <Tag color="green">Đã duyệt</Tag>;
      case 'Rejected':
        return <Tag color="red">Đã từ chối</Tag>;
      default:
        return <Tag>{status}</Tag>;
    }
  };

  const columns = [
    {
      title: 'Người yêu cầu',
      dataIndex: 'requestorName',
      key: 'requestorName',
    },
    {
      title: 'Ngày yêu cầu',
      dataIndex: 'requestDate',
      key: 'requestDate',
    },
    {
      title: 'Số lượng sách',
      dataIndex: 'bookCount',
      key: 'bookCount',
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
      render: (text: string, record: BorrowRequest) => (
        record.status === 'Pending' ? (
          <span>
            <Button
              type="primary"
              icon={<CheckOutlined />}
              onClick={() => handleApprove(record.id)}
              style={{ marginRight: 8 }}
            >
              Duyệt
            </Button>
            <Button
              danger
              icon={<CloseOutlined />}
              onClick={() => handleReject(record.id)}
            >
              Từ chối
            </Button>
          </span>
        ) : null
      ),
    },
  ];

  return (
    <div>
      <Table
        dataSource={requests}
        columns={columns}
        rowKey="id"
        loading={loading}
      />
    </div>
  );
};

export default BorrowRequestManagement; 