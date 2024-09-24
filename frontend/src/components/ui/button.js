import React from 'react';

const Button = ({ children, size, className, ...props }) => {
  return (
    <button
      className={`px-4 py-2 rounded ${size === 'lg' ? 'text-lg' : ''} ${className}`}
      {...props}
    >
      {children}
    </button>
  );
};

export default Button;
