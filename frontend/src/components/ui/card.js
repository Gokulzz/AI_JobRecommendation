import React from 'react';

export const Card = ({ children, className }) => {
  return (
    <div className={`bg-white shadow-md rounded ${className}`}>
      {children}
    </div>
  );
};

export const CardContent = ({ children, className }) => {
  return (
    <div className={`p-4 ${className}`}>
      {children}
    </div>
  );
};