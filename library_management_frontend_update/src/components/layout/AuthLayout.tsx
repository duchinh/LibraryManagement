import React from 'react';

interface AuthLayoutProps {
  children: React.ReactNode;
  backgroundImage?: string;
}

export const AuthLayout: React.FC<AuthLayoutProps> = ({ children, backgroundImage }) => {
  return (
    <div className="min-h-screen flex">
      {/* Left side - Background image */}
      <div 
        className="hidden lg:block lg:w-1/2 bg-cover bg-center"
        style={{ backgroundImage: `url(${backgroundImage})` }}
      >
        <div className="h-full bg-black bg-opacity-40 flex items-center justify-center">
          <div className="text-white text-center p-8">
            <h1 className="text-4xl font-bold mb-4">Thư viện trực tuyến</h1>
            <p className="text-lg">Khám phá thế giới tri thức cùng chúng tôi</p>
          </div>
        </div>
      </div>

      {/* Right side - Auth form */}
      <div className="w-full lg:w-1/2 flex items-center justify-center p-8">
        {children}
      </div>
    </div>
  );
}; 