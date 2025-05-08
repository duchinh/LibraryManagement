import React from 'react';

interface AuthLayoutProps {
  children: React.ReactNode;
  backgroundImage?: string;
}

const AuthLayout: React.FC<AuthLayoutProps> = ({ children, backgroundImage }) => {
  return (
    <div className="min-h-screen flex">
      {/* Left side - Background image */}
      <div 
        className="w-full lg:w-1/2 bg-cover bg-center"
        style={{ backgroundImage: `url(${backgroundImage})`}}
      >
      </div>

      {/* Right side - Auth form */}
      <div className="w-full lg:w-1/2 flex items-center justify-center p-8">
        {children}
      </div>
    </div>
  );
};

export default AuthLayout;