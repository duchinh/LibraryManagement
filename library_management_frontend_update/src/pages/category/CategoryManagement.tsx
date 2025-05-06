import React, { useState, useEffect } from 'react';
import { Button, Table, Modal, Form, Input, message } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { Category } from '../../interfaces';
import { categoryService } from '../../services/category.service';

const CategoryManagement: React.FC = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingCategory, setEditingCategory] = useState<Category | null>(null);
  const [form] = Form.useForm();

  useEffect(() => {
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    try {
      const response = await categoryService.getAllCategories();
      setCategories(response.data);
    } catch (error) {
      message.error('Không thể tải danh sách danh mục');
    }
  };

  const handleAdd = () => {
    setEditingCategory(null);
    form.resetFields();
    setIsModalVisible(true);
  };

  const handleEdit = (category: Category) => {
    setEditingCategory(category);
    form.setFieldsValue(category);
    setIsModalVisible(true);
  };

  const handleDelete = async (id: string) => {
    Modal.confirm({
      title: 'Xác nhận xóa',
      content: 'Bạn có chắc chắn muốn xóa danh mục này?',
      onOk: async () => {
        try {
          await categoryService.deleteCategory(id);
          message.success('Xóa danh mục thành công');
          fetchCategories();
        } catch (error) {
          message.error('Xóa danh mục thất bại');
        }
      },
    });
  };

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      if (editingCategory) {
        await categoryService.updateCategory(editingCategory.id, values);
        message.success('Cập nhật danh mục thành công');
      } else {
        await categoryService.createCategory(values);
        message.success('Thêm danh mục thành công');
      }
      setIsModalVisible(false);
      fetchCategories();
    } catch (error) {
      message.error('Thao tác thất bại');
    }
  };

  const columns = [
    {
      title: 'Tên danh mục',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Mô tả',
      dataIndex: 'description',
      key: 'description',
    },
    {
      title: 'Số lượng sách',
      dataIndex: 'bookCount',
      key: 'bookCount',
    },
    {
      title: 'Thao tác',
      key: 'action',
      render: (text: string, record: Category) => (
        <span>
          <Button type="link" onClick={() => handleEdit(record)}>
            <EditOutlined />
          </Button>
          <Button type="link" danger onClick={() => handleDelete(record.id)}>
            <DeleteOutlined />
          </Button>
        </span>
      ),
    },
  ];

  return (
    <div>
      <div style={{ marginBottom: 16 }}>
        <Button type="primary" onClick={handleAdd} icon={<PlusOutlined />}>
          Thêm danh mục
        </Button>
      </div>

      <Table dataSource={categories} columns={columns} rowKey="id" />

      <Modal
        title={editingCategory ? 'Cập nhật danh mục' : 'Thêm danh mục mới'}
        visible={isModalVisible}
        onOk={handleSubmit}
        onCancel={() => setIsModalVisible(false)}
      >
        <Form form={form} layout="vertical">
          <Form.Item
            name="name"
            label="Tên danh mục"
            rules={[{ required: true, message: 'Vui lòng nhập tên danh mục' }]}
          >
            <Input />
          </Form.Item>

          <Form.Item
            name="description"
            label="Mô tả"
          >
            <Input.TextArea />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default CategoryManagement; 