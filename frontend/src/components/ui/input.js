import React from 'react';
const Input = ({ className, ...props }) => {
    return <input className={`w-full p-3 ${className}`} {...props} />;
  };
  
  export default Input;
  