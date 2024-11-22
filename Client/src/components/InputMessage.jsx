const InputMessage = ({ children }) => {
  return (
    <>
      {children ? (
        <p className="visible text-sm">{children}</p>
      ) : (
        <p className="invisible text-sm">[Placeholder]</p>
      )}
    </>
  );
};

export default InputMessage;
